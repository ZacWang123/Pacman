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
    public float updateInterval = 1f;

    string[] phases = new string[8] { "Scatter", "Chase", "Scatter", "Chase", "Scatter", "Chase", "Scatter", "Chase"};
    int[] phaseDuration = new int[8] { 100, 20, 7, 20, 5, 20, 5, 999};
    string currentPhase = "";

/*    int[] ghostSpawnTime = new int[4] { 1, 1000, 1000, 1000};*/

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
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ghosts[j].mode = phases[i];
                currentPhase = phases[i];
            }
            yield return new WaitForSeconds(phaseDuration[i]);
        }
    }

    public void GhostMovement()
    {
        for (int i = 0; i < 2; i++)
        {
            bool inScatter = false;
            if (currentPhase == "Scatter")
            {
                int scatterLength = ghosts[i].scatterPos.Length;
                for (int j = 0; j < scatterLength; j++)
                {
                    if (ghosts[i].ghostPosition.Row == ghosts[i].scatterPos[j].Row && ghosts[i].ghostPosition.Col == ghosts[i].scatterPos[j].Col)
                    {
                        UpdateGhost(ghosts[i].scatterPos[(j + 1) % scatterLength].Row, ghosts[i].scatterPos[(j + 1) % scatterLength].Col, i);
                        inScatter = true;
                        break;
                    }
                }

                if (!inScatter) {
                    Dictionary<string, Positions> neighbors = NeighboringCells(ghosts[i].ghostPosition, ghosts[i].direction);
                    Positions bestNeighbor = BestNeighbor(neighbors, i);
                    UpdateGhost(bestNeighbor.Row, bestNeighbor.Col, i);
                }
            }
        }
    }

    public void UpdateGhost(int row, int col, int ghostNum) {
        grid.UpdateGrid(ghosts[ghostNum].ghostPosition.Row, ghosts[ghostNum].ghostPosition.Col, ghosts[ghostNum].currentCellValue);
        ghosts[ghostNum].ghostPosition.Row = row;
        ghosts[ghostNum].ghostPosition.Col = col;
        ghosts[ghostNum].currentCellValue = grid.GetGridCell(ghosts[ghostNum].ghostPosition.Row, ghosts[ghostNum].ghostPosition.Col);
        grid.UpdateGrid(ghosts[ghostNum].ghostPosition.Row, ghosts[ghostNum].ghostPosition.Col, ghosts[ghostNum].Id);
    }

    public Positions BestNeighbor(Dictionary<string, Positions> neighbors, int ghostNum)
    {
        Positions bestNeighbor = new Positions(0, 0);
        double distance = 100000.0;
        string direction = "";

        foreach (KeyValuePair<string, Positions> keyValue in neighbors)
        {
            Positions target = new Positions(0, 0);
            if (ghosts[ghostNum].exited && grid.GetGridCell(keyValue.Value.Row, keyValue.Value.Col) == 2) {
                continue;
            }

            if (ghosts[ghostNum].exited == false) {
                target.Row = 12;
                target.Col = 19;
                if (ghosts[ghostNum].ghostPosition.Row == 12 && ghosts[ghostNum].ghostPosition.Col == 19) {
                    ghosts[ghostNum].exited = true;
                    target.Row = ghosts[ghostNum].scatterTarget.Row;
                    target.Col = ghosts[ghostNum].scatterTarget.Col;
                }
            }
            else if (currentPhase == "Scatter")
            {
                target.Row = ghosts[ghostNum].scatterTarget.Row;
                target.Col = ghosts[ghostNum].scatterTarget.Col;
            }
            else if (currentPhase == "Chase")
            {
                target.Row = pacman.pacmanPosition.Row;
                target.Col = pacman.pacmanPosition.Col;
            }
            double newDistance = Math.Sqrt(Math.Pow(keyValue.Value.Row - target.Row, 2) + Math.Pow(keyValue.Value.Col - target.Col, 2));

            if (newDistance < distance)
            {
                distance = newDistance;
                bestNeighbor.Row = keyValue.Value.Row;
                bestNeighbor.Col = keyValue.Value.Col;
                direction = keyValue.Key;
            }
        }
        ghosts[ghostNum].direction = direction;
        return bestNeighbor;
    }

    public Dictionary<string, Positions> NeighboringCells(Positions currentCell, string direction)
    {
        Dictionary<string, Positions> neighbors = new Dictionary<string, Positions>();
        if (direction != "up")
        {
            if (grid.WithinGrid(currentCell.Row, currentCell.Col - 1) && grid.GetGridCell(currentCell.Row, currentCell.Col - 1) != 1)
            {
                neighbors.Add("down", new Positions(currentCell.Row, currentCell.Col - 1));
            }
        }
        if (direction != "down")
        {
            if (grid.WithinGrid(currentCell.Row, currentCell.Col + 1) && grid.GetGridCell(currentCell.Row, currentCell.Col + 1) != 1)
            {
                neighbors.Add("up", new Positions(currentCell.Row, currentCell.Col + 1));
            }
        }
        if (direction != "left")
        {
            if (grid.WithinGrid(currentCell.Row + 1, currentCell.Col) && grid.GetGridCell(currentCell.Row + 1, currentCell.Col) != 1)
            {
                neighbors.Add("right", new Positions(currentCell.Row + 1, currentCell.Col));
            }
        }
        if (direction != "right")
        {
            if (grid.WithinGrid(currentCell.Row - 1, currentCell.Col) && grid.GetGridCell(currentCell.Row - 1, currentCell.Col) != 1)
            {
                neighbors.Add("left", new Positions(currentCell.Row - 1, currentCell.Col));
            }
        }

        return neighbors;
    }

    /*if (currentPhase == "Scatter") {
        int scatterLength = ghost2.scatterPos.Length;
        for (int i = 0; i < scatterLength; i++) {
            if (ghost2.ghostPosition.Row == ghost2.scatterPos[i].Row && ghost2.ghostPosition.Col == ghost2.scatterPos[i].Col) {
                grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, 0);
                ghost2.ghostPosition.Row = ghost2.scatterPos[(i + 1) % scatterLength].Row;
                ghost2.ghostPosition.Col = ghost2.scatterPos[(i + 1) % scatterLength].Col;
                grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, ghost2.Id);
                return;
            }
        }
    }

    Dictionary<string, Positions> neighbors = NeighboringCells(ghost2.ghostPosition, ghost2.direction);
    Positions bestNeighbor = BestNeighbor(neighbors);
    grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, 0);
    ghost2.ghostPosition.Row = bestNeighbor.Row;
    ghost2.ghostPosition.Col = bestNeighbor.Col;
    grid.UpdateGrid(ghost2.ghostPosition.Row, ghost2.ghostPosition.Col, ghost2.Id);*/
    /*public Positions BestNeighbor(Dictionary<string, Positions> neighbors, int ghostNum)
    {
        Positions bestNeighbor = new Positions(0, 0);
        double distance = 100000.0;
        string direction = "";

        foreach (KeyValuePair<string, Positions> keyValue in neighbors)
        {
            Positions target = new Positions(0, 0);
            if (currentPhase == "Scatter")
            {
                target.Row = ghost2.scatterTarget.Row;
                target.Col = ghost2.scatterTarget.Col;
            }
            else if (currentPhase == "Chase")
            {
                target.Row = pacman.pacmanPosition.Row;
                target.Col = pacman.pacmanPosition.Col;
            }
            double newDistance = Math.Sqrt(Math.Pow(keyValue.Value.Row - target.Row, 2) + Math.Pow(keyValue.Value.Col - target.Col, 2));

            if (newDistance < distance)
            {
                distance = newDistance;
                bestNeighbor.Row = keyValue.Value.Row;
                bestNeighbor.Col = keyValue.Value.Col;
                direction = keyValue.Key;
            }
        }
        ghost2.direction = direction;
        return bestNeighbor;
    }*/

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
