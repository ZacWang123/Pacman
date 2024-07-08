using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Cell;
    public GameGrid grid;
    public Pacman pacman;
    public Ghost1 ghost1;
    public Ghost2 ghost2;   
    public Ghost3 ghost3;
    public Ghost4 ghost4;

    void Start()
    {
        grid = new GameGrid(Cell);
        pacman = new Pacman();
        ghost1 = new Ghost1();
        ghost2 = new Ghost2();
        ghost3 = new Ghost3();
        ghost4 = new Ghost4();

        UpdatePacmanPosition(13,7);
        UpdateGhostPositions();
        grid.DrawGrid();
        grid.UpdateGridColour();
    }

    public void UpdatePacmanPosition(int oldRow, int oldCol) {
        grid.UpdateGrid(oldRow, oldCol, 0);
        grid.UpdateGrid(pacman.pacmanPosition.Row, pacman.pacmanPosition.Col, pacman.Id);
    }

    public void PacmanMovement()
    {
        Positions oldpacmanPosition = pacman.pacmanPosition;
        Positions newpacmanPosition = new Positions(oldpacmanPosition.Row, oldpacmanPosition.Col);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newpacmanPosition.Row -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newpacmanPosition.Row += 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newpacmanPosition.Col -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newpacmanPosition.Col += 1;
        }

        if (newpacmanPosition.Row == -1 && newpacmanPosition.Col == 16) {
            newpacmanPosition.Row = 27;

            pacman.pacmanPosition = newpacmanPosition;
            UpdatePacmanPosition(oldpacmanPosition.Row, oldpacmanPosition.Col);
        }
        else if (newpacmanPosition.Row == 28 && newpacmanPosition.Col == 16)
        {
            newpacmanPosition.Row = 0;
            pacman.pacmanPosition = newpacmanPosition;
            UpdatePacmanPosition(oldpacmanPosition.Row, oldpacmanPosition.Col);
        }
        else if (grid.GetGridCell(newpacmanPosition.Row, newpacmanPosition.Col) != 1)
        {
            pacman.pacmanPosition = newpacmanPosition;
            UpdatePacmanPosition(oldpacmanPosition.Row, oldpacmanPosition.Col);
        }
    }

    public void UpdateGhostPositions() {
        grid.UpdateGrid(ghost1.ghostPosition.Row, ghost1.ghostPosition.Col, ghost1.Id);
        grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, ghost2.Id);
        grid.UpdateGrid(ghost3.ghostPosition.Row, ghost3.ghostPosition.Col, ghost3.Id);
        grid.UpdateGrid(ghost4.ghostPosition.Row, ghost4.ghostPosition.Col, ghost4.Id);
    }
    void Update()
    {
        PacmanMovement();
        grid.UpdateGridColour();
    }
}
