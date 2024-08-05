using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost
{
    public Positions[] scatterPos;
    public Positions ghostPosition;
    public int Id;
    public string mode;
    public string direction;
    public Positions scatterTarget;
    public bool exited = false;
    public int currentCellValue;
    public bool active = false;
}
