using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Cell;
    public GameGrid grid;
    public Pacman pacman;
    public Ghost[] ghosts;
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
        ghosts = new Ghost[] { ghost1, ghost2, ghost3, ghost4 };

        UpdatePacmanPosition(13,7);
        SpawnGhosts();
        grid.DrawGrid();
        grid.UpdateGridColour();

        StartGamePhases();
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

    public void SpawnGhosts() {
        for (int i = 0; i < ghosts.Length; i++) {
            grid.UpdateGrid(ghosts[i].ghostPosition.Row, ghosts[i].ghostPosition.Col, ghosts[i].Id);
        }
    }
    public void StartGamePhases() {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop() {
        string[] phases = new string[8] { "Scatter", "Chase", "Scatter", "Chase", "Scatter", "Chase", "Scatter", "Chase", };
        int[] duration = new int[8] { 7, 20, 7, 20, 5, 20, 5, 999 };

        for (int i = 0; i < 8; i++)
        {
            print(phases[i]);
            yield return new WaitForSeconds(duration[i]);
        }
    }

    void Update()
    {
        PacmanMovement();
        grid.UpdateGridColour();
    }
}
