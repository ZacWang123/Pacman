using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost1 : Ghost
{
    public Ghost1()
    {
        ghostPosition = new Positions(13, 19);
        Id = 4;
        direction = "up";
        exited = true;
        scatterTarget = new Positions(21, 25);
        scatterPos = new Positions[18] {
            new Positions(21, 29),
            new Positions(22, 29),
            new Positions(23, 29),
            new Positions(24, 29),
            new Positions(25, 29),
            new Positions(26, 29),
            new Positions(26, 28),
            new Positions(26, 27),
            new Positions(26, 26),
            new Positions(26, 25),
            new Positions(25, 25),
            new Positions(24, 25),
            new Positions(23, 25),
            new Positions(22, 25),
            new Positions(21, 25),
            new Positions(21, 26),
            new Positions(21, 27),
            new Positions(21, 28),
        };
    }
}