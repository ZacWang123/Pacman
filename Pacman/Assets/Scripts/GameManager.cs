using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float time;
    public float updateInterval = 0.5f;

    void Start()
    {
        grid = new GameGrid(Cell);
        pacman = new Pacman();
        ghost1 = new Ghost1();
        ghost2 = new Ghost2();
        ghost3 = new Ghost3();
        ghost4 = new Ghost4();
        ghosts = new Ghost[] { ghost1, ghost2, ghost3, ghost4 };

        UpdatePacmanPosition(13, 7);
        SpawnGhosts();
        grid.DrawGrid();
        grid.UpdateGridColour();

        StartGamePhases();
    }

    public void UpdatePacmanPosition(int oldRow, int oldCol)
    {
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
            pacman.Direction = "left";
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newpacmanPosition.Row += 1;
            pacman.Direction = "right";
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newpacmanPosition.Col -= 1;
            pacman.Direction = "down";
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newpacmanPosition.Col += 1;
            pacman.Direction = "up";
        }

        if (newpacmanPosition.Row == -1 && newpacmanPosition.Col == 16)
        {
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

    public void SpawnGhosts()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            grid.UpdateGrid(ghosts[i].ghostPosition.Row, ghosts[i].ghostPosition.Col, ghosts[i].Id);
        }
    }
    public void StartGamePhases()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        string[] phases = new string[8] { "Scatter", "Chase", "Scatter", "Chase", "Scatter", "Chase", "Scatter", "Chase", };
        int[] duration = new int[8] { 1000, 20, 7, 20, 5, 20, 5, 999 };
        /*int[] duration = new int[8] { 7, 20, 7, 20, 5, 20, 5, 999 };*/

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ghosts[j].mode = phases[i];
            }
            yield return new WaitForSeconds(duration[i]);
        }
    }

    public void GhostMovement()
    {
        Dictionary<string, Positions> neighbors = NeighboringCells(ghost2.ghostPosition, ghost2.direction);
        Positions bestNeighbor = BestNeighbor(neighbors);
        grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, 0);
        ghost2.ghostPosition.Row = bestNeighbor.Row;
        ghost2.ghostPosition.Col = bestNeighbor.Col;
        grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, ghost2.Id);
        /*Positions[] nextGhostMove = ghost2.NextPosition(pacman.pacmanPosition);

        for (int i = 0; i < nextGhostMove.Length; i++)
        {
            if (grid.WithinGrid(nextGhostMove[i].Row, nextGhostMove[i].Col))
            {
                grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, 0);
                ghost2.ghostPosition = new Positions(nextGhostMove[i].Row, nextGhostMove[i].Col);
                grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, ghost2.Id);
                break;
            }
        }*/
    }

    public Positions BestNeighbor(Dictionary<string, Positions> neighbors) {
        Positions bestNeighbor = new Positions(0, 0);
        double distance = 100000.0;
        string direction = "";

        foreach (KeyValuePair<string, Positions> keyValue in neighbors) {
            double newDistance = Math.Sqrt(Math.Pow(keyValue.Value.Row - pacman.pacmanPosition.Row, 2) + Math.Pow(keyValue.Value.Col - pacman.pacmanPosition.Col, 2));
            if (newDistance < distance) {
                distance = newDistance;
                bestNeighbor.Row = keyValue.Value.Row;
                bestNeighbor.Col = keyValue.Value.Col;
                direction = keyValue.Key;
            }
        }
        ghost2.direction = direction;
        return bestNeighbor;
    }

    public Dictionary<string, Positions> NeighboringCells(Positions currentCell, string direction)
    {
        Dictionary<string, Positions> neighbors = new Dictionary<string, Positions>();
        if (direction != "up") {
            if (grid.WithinGrid(currentCell.Row, currentCell.Col - 1) && grid.GetGridCell(currentCell.Row, currentCell.Col - 1) != 1) {
                neighbors.Add("down", new Positions(currentCell.Row, currentCell.Col - 1));
            }
        }
        if (direction != "down") {
            if (grid.WithinGrid(currentCell.Row, currentCell.Col + 1) && grid.GetGridCell(currentCell.Row, currentCell.Col + 1) != 1) {
                    neighbors.Add("up", new Positions(currentCell.Row, currentCell.Col + 1));
            }
        }
        if (direction != "left") {
            if (grid.WithinGrid(currentCell.Row + 1, currentCell.Col) && grid.GetGridCell(currentCell.Row + 1, currentCell.Col) != 1) {
                neighbors.Add("right", new Positions(currentCell.Row + 1, currentCell.Col));
            }
        }
        if (direction != "right") {
            if (grid.WithinGrid(currentCell.Row - 1, currentCell.Col) && grid.GetGridCell(currentCell.Row - 1, currentCell.Col) != 1) {
                neighbors.Add("left", new Positions(currentCell.Row - 1, currentCell.Col));
            }
        }

        return neighbors;
    }

    void Update()
    {
        time += Time.deltaTime;
        grid.UpdateGridColour();
        PacmanMovement();
        if (time > updateInterval)
        {
            GhostMovement();
            time = 0f;
        }
    }
}
