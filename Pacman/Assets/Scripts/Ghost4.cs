using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost4 : Ghost
{
    public Ghost4()
    {
        ghostPosition = new Positions(16, 16);
        Id = 7;
        direction = "up";
        exited = false;
        scatterTarget = new Positions(21, 25);
        scatterPos = new Positions[34] {
            new Positions(6, 7),
            new Positions(6, 6),
            new Positions(6, 5),
            new Positions(6, 4),
            new Positions(5, 4),
            new Positions(4, 4),
            new Positions(3, 4),
            new Positions(2, 4),
            new Positions(1, 4),
            new Positions(1, 3),
            new Positions(1, 2),
            new Positions(1, 1),
            new Positions(2, 1),
            new Positions(3, 1),
            new Positions(4, 1),
            new Positions(5, 1),
            new Positions(6, 1),
            new Positions(7, 1),
            new Positions(8, 1),
            new Positions(9, 1),
            new Positions(10, 1),
            new Positions(11, 1),
            new Positions(12, 1),
            new Positions(12, 2),
            new Positions(12, 3),
            new Positions(12, 4),
            new Positions(11, 4),
            new Positions(10, 4),
            new Positions(9, 4),
            new Positions(9, 5),
            new Positions(9, 6),
            new Positions(9, 7),
            new Positions(8, 7),
            new Positions(7, 7),
        };
    }
}

