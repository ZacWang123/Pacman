using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Pacman
{
    public Positions pacmanPosition;
    public int Id {
        get;
    }

    public Pacman() {
        pacmanPosition = new Positions(13, 19);
        Id = 3;
    }

/*    public void Movement(string Direction) {
        switch (Direction)
        {
            case "Up":
                pacmanPosition.Col += 1;
                break;
            case "Down":
                pacmanPosition.Col -= 1;
                break;
            case "Left":
                pacmanPosition.Row -= 1;
                break;
            case "Right":
                pacmanPosition.Row += 1;
                break;
        }
    }*/
}
