using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool isAlive = false;
    private bool isLider = false;
    private bool isWarrior = false;

    public int generationsLivedCount = 0;
    public int numNeighbors = 0;

    private SpriteRenderer cellSprite;

    void Awake()
    {
        cellSprite = GetComponent<SpriteRenderer>();
        cellSprite.sprite = Resources.Load<Sprite>("Celltextures/DefaultCell");
    }

    public void SetAlive (bool alive)
    {
        isAlive = alive;
        cellSprite.enabled = alive;
        
        if (!isAlive) {
            generationsLivedCount = 0;
            MakeDefault();
        }
    }

    public void MakeLider()
    {
        isWarrior = false;
        isLider = true;
        cellSprite.sprite = Resources.Load<Sprite>("Celltextures/LiderCell");
    }

    public void MakeWarrior()
    {
        isWarrior = true;
        isLider = false;
        cellSprite.sprite = Resources.Load<Sprite>("Celltextures/WarriorCell");
    }

    public void MakeDefault()
    {
        isWarrior = false;
        isLider = false;
        cellSprite.sprite = Resources.Load<Sprite>("Celltextures/DefaultCell");
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public bool IsLider()
    {
        return isLider;
    }

    public bool IsWarrior() {
        return isWarrior;
    }
}
