press "SPACEBAR" to start the simulation.
press "R" to generate random cells.
cells can be placed by left clicking on the screen only when the game is paused.
right click resets the simulation when the game is paused.
the type of environments can be changed either during the game or when the game is paused.

DEFAULT ZONE:
- default game of life, roles do not affect the cells

(just in case the user changes the environment during the game - to add some variation)
- a cell will become a lider if it lives longer than 15 generations
- a cell has a 1% chance to mutate into a warrior


FERTILE ZONE:
- warriors die instantly in fertility zone
- a cell needs at least 1 and at most 3 neighbours to reproduce

DEATH ZONE:
- a cell needs either 3 or 4 neighbours to reproduce
- default cells will live for at most 15 generations,
  lider cells will live for at most 20 generations,
  warrior cells can live indefinetely
- a lider cell has a 10 percent chance to be born
- a default cell has a 30% chance to become a warrior or  it dies.
  liders have a 60% chance to become warriors
- a default cell has a 30% chance to become a warrior during a generation.
  if not, it has a 2% chance to die
- a lider cell has a 60% chance to become a warrior during a generation.
  if not, it has a 2% chance to die

ANARCHY ZONE:
- each generation the role of each cell that is in this zone will change
- any cell has a 33% chance to become a warrior, a 33% chance to become a lider, or a 33% change to become a default cell

