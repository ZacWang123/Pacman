using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Cell;
    public GameGrid grid;

    void Start()
    {
        grid = new GameGrid(Cell);
        grid.DrawGrid();
        grid.UpdateGridColour();
    }

    void Update()
    {
    }
}
