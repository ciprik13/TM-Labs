using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static int width = 128;
    private static int height = 72;

    private bool isSimulationPaused = true;

    public Dropdown ddRoleSelector;

    public Dropdown ddEnvironment1;
    public Dropdown ddEnvironment2;
    public Dropdown ddEnvironment3;
    public Dropdown ddEnvironment4;

    private Cell[,] grid = new Cell[width, height];

    // Start is called before the first frame update
    void Start()
    {
        ddRoleSelector = GameObject.Find("RoleSelector").GetComponent<Dropdown>();
        ddEnvironment1 = GameObject.Find("EnvironmentTopLeft").GetComponent<Dropdown>();
        ddEnvironment2 = GameObject.Find("EnvironmentTopRight").GetComponent<Dropdown>();
        ddEnvironment3 = GameObject.Find("EnvironmentBottomLeft").GetComponent<Dropdown>();
        ddEnvironment4 = GameObject.Find("EnvironmentBottomRight").GetComponent<Dropdown>();
    
        PlaceCells();
        InvokeRepeating("makeUpdates", 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        ReadUserInput();
    }

    void makeUpdates()
    {
        if (isSimulationPaused) return;

        CountNeighbors();
        NextGeneration();
    }

    void ReadUserInput()
    {
        if (isSimulationPaused) {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.RoundToInt(mousePoint.x);
                int y = Mathf.RoundToInt(mousePoint.y);

                if (0 <= x && x < width && 0 <= y && y < height) {
                    if (ddRoleSelector.value == 1) {
                        grid[x, y].MakeLider();
                    }
                    else if (ddRoleSelector.value == 2) {
                        grid[x, y].MakeWarrior();
                    }
                    else {
                        grid[x, y].MakeDefault();
                    }

                    grid[x, y].SetAlive(!grid[x, y].IsAlive());
                }
            }

            if (Input.GetMouseButtonDown(1)) {
                ResetCells();
            }

            if (Input.GetKeyUp(KeyCode.R)) {
                ResetCells();
                for (int i = 0; i < height; i++) {
                    for (int j = 0; j < width; j++) {
                        grid[j, i].SetAlive(hasProbability(50));
                    }
                }
            }
        }

        if (Input.GetKeyUp("space")) {
            isSimulationPaused = !isSimulationPaused;
        }
    }

    void ResetCells()
    {
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                grid[j, i].SetAlive(false);
            }
        }
        grid = new Cell[width, height];
        PlaceCells();
    }

    void PlaceCells()
    {
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                Cell cell = Instantiate(Resources.Load("Cell", typeof(Cell)), new Vector2(j, i), Quaternion.identity) as Cell;
                grid[j, i] = cell;
                grid[j, i].SetAlive(false);
            }
        }
    }

    void NextGeneration()
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (0 <= i && i < width / 2 && height / 2 <= j && j < height) {
                    ChooseRulesToApply(ddEnvironment1.value, i, j);
                }
                else if (width / 2 <= i && i < width && height / 2 <= j && j < height) {
                    ChooseRulesToApply(ddEnvironment2.value, i, j);
                }
                else if (0 <= i && i < width / 2 && 0 <= j && j < height / 2) {
                    ChooseRulesToApply(ddEnvironment3.value, i, j);
                }
                else if (width / 2 <= i && i < width && 0 <= j && j < height / 2) {
                    ChooseRulesToApply(ddEnvironment4.value, i, j);
                }
            }
        }
    }

    void ChooseRulesToApply(int environment, int i, int j)
    {
        switch (environment) {
            case 0:
                ApplyDefaultRules(i, j);
                break;
            case 1:
                ApplyAnarchyZoneRules(i, j);
                break;
            case 2:
                ApplyDeathZoneRules(i, j);
                break;
            case 3:
                ApplyFertileZoneRules(i, j);
                break;
        }
    }

    void ApplyDefaultRules(int i, int j)
    {
        if (grid[i, j].numNeighbors == 2 && grid[i, j].IsAlive()) {
            grid[i, j].generationsLivedCount++;
        }
        else if (grid[i, j].numNeighbors == 3) {
            grid[i, j].SetAlive(true);
            grid[i, j].generationsLivedCount++;
        }
        else {
            grid[i, j].SetAlive(false);
        }

        if (grid[i, j].IsAlive() && !grid[i, j].IsLider() && hasProbability(5)) {
            grid[i, j].MakeWarrior();
        }

        if (grid[i, j].IsAlive() && grid[i, j].generationsLivedCount == 15 && !grid[i, j].IsWarrior()) {
            grid[i, j].MakeLider();
        }
    }

    void ApplyAnarchyZoneRules(int i, int j)
    {
        if (grid[i, j].numNeighbors == 2 && grid[i, j].IsAlive()) {
            grid[i, j].generationsLivedCount++;
        }
        else if (grid[i, j].numNeighbors == 2 || grid[i, j].numNeighbors == 3) {
            grid[i, j].SetAlive(true);
            grid[i, j].generationsLivedCount++;
        }
        else {
            grid[i, j].SetAlive(false);
        }

        if (grid[i, j].IsAlive()) {
            if (hasProbability(33)) {
                grid[i, j].MakeWarrior();
            }
            else if (hasProbability(33)) {
                grid[i, j].MakeLider();
            }
            else {
                grid[i, j].MakeDefault();
            }
        }
    }

    void ApplyDeathZoneRules(int i, int j)
    {
        if (grid[i, j].numNeighbors == 2 && grid[i, j].IsAlive()) {
            grid[i, j].generationsLivedCount++;
        }
        else if (grid[i, j].numNeighbors == 3 || grid[i, j].numNeighbors == 4) {
            grid[i, j].SetAlive(true);
            if (hasProbability(10)) {
                grid[i, j].MakeLider();
            }
            grid[i, j].generationsLivedCount++;
        }
        else {
            grid[i, j].SetAlive(false);
        }

        if (!grid[i, j].IsWarrior() && !grid[i, j].IsLider() && grid[i, j].generationsLivedCount > 15) {
            grid[i, j].SetAlive(false);
        }
        else if (grid[i, j].IsLider() && grid[i, j].generationsLivedCount > 20) {
            grid[i, j].SetAlive(false);
        }

        if (!grid[i, j].IsWarrior() && grid[i, j].IsAlive()) {
            if (!grid[i, j].IsLider()) {
                if (hasProbability(30)) {
                    grid[i, j].MakeWarrior();
                }
                else if (hasProbability(2)){
                    grid[i, j].SetAlive(false);
                }
            }
            else {
                if (hasProbability(60)) {
                    grid[i, j].MakeWarrior();
                }
                else if (hasProbability(2)) {
                    grid[i, j].SetAlive(false);
                }
            }
        }
    }

    void ApplyFertileZoneRules(int i, int j)
    {
        if (grid[i, j].IsWarrior()) {
            grid[i, j].SetAlive(false);
            grid[i, j].numNeighbors = 0;
        }

        if (grid[i, j].numNeighbors == 2 && grid[i, j].IsAlive()) {
            grid[i, j].generationsLivedCount++;
        }
        else if (1 <= grid[i, j].numNeighbors && grid[i, j].numNeighbors <= 3) {
            grid[i, j].SetAlive(true);
            grid[i, j].generationsLivedCount++;
        }
        else {
            grid[i, j].SetAlive(false);
        }

        if (grid[i, j].generationsLivedCount == 15) {
            grid[i, j].MakeLider();
        }
    }

    void CountNeighbors()
    {
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                int nr = 0;

                if (i + 1 < height && grid[j, i + 1].IsAlive()) {
                    nr++;
                }

                if (i - 1 >= 0 && grid[j, i - 1].IsAlive()) {
                    nr++;
                }

                if (j + 1 < width && grid[j + 1, i].IsAlive()) {
                    nr++;
                }

                if (j - 1 >= 0 && grid[j - 1, i].IsAlive()) {
                    nr++;
                }

                if (i + 1 < height && j + 1 < width && grid[j + 1, i + 1].IsAlive()){
                    nr++;
                }

                if (i - 1 >= 0 && j - 1 >= 0 && grid[j - 1, i - 1].IsAlive()) {
                    nr++;
                }

                if (i + 1 < height && j - 1 >= 0 && grid[j - 1, i + 1].IsAlive()) {
                    nr++;
                }

                if (i - 1 >= 0 && j + 1 < width && grid[j + 1, i - 1].IsAlive()) {
                    nr++;
                }

                grid[j, i].numNeighbors = nr;
            }
        }
    }

    bool hasProbability(int percent)
    {
        return 100 - percent < UnityEngine.Random.Range(1, 100);
    }
}
