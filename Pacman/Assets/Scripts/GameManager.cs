using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Cell;
    public GameGrid grid;
    public Pacman pacman;

    void Start()
    {
        grid = new GameGrid(Cell);
        pacman = new Pacman();

        UpdatePacmanPosition(13,19);
        grid.DrawGrid();
        grid.UpdateGridColour();
    }

    public void UpdatePacmanPosition(int oldRow, int oldCol) {
        grid.UpdateGrid(oldRow, oldCol, 0);
        grid.UpdateGrid(pacman.pacmanPosition.Row, pacman.pacmanPosition.Col, pacman.Id);
    }

    public void PacmanMovement()
    {
        Positions pacmanPosition = pacman.pacmanPosition;
        Positions newpacmanPosition = new Positions(pacmanPosition.Row, pacmanPosition.Col);

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
            UpdatePacmanPosition(pacmanPosition.Row, pacmanPosition.Col);
        }
        else if (newpacmanPosition.Row == 28 && newpacmanPosition.Col == 16)
        {
            newpacmanPosition.Row = 0;
            pacman.pacmanPosition = newpacmanPosition;
            UpdatePacmanPosition(pacmanPosition.Row, pacmanPosition.Col);
        }
        else if (grid.GetGridCell(newpacmanPosition.Row, newpacmanPosition.Col) != 1)
        {
            pacman.pacmanPosition = newpacmanPosition;
            UpdatePacmanPosition(pacmanPosition.Row, pacmanPosition.Col);
        }
    }
    void Update()
    {
        PacmanMovement();
        grid.UpdateGridColour();
    }
}
