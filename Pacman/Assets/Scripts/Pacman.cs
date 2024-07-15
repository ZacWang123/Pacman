using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Pacman
{
    public Positions pacmanPosition;
    public string Direction;

    public int Id {
        get;
    }

    public Pacman() {
        pacmanPosition = new Positions(13, 7);
        Id = 3;
    }
}
