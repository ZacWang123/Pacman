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
        scatterTarget = new Positions(18, 7);
        scatterPos = new Positions[34] {
            new Positions(18, 7),
            new Positions(19, 7),
            new Positions(20, 7),
            new Positions(21, 7),
            new Positions(21, 6),
            new Positions(21, 5),
            new Positions(21, 4),
            new Positions(22, 4),
            new Positions(23, 4),
            new Positions(24, 4),
            new Positions(25, 4),
            new Positions(26, 4),
            new Positions(26, 3),
            new Positions(26, 2),
            new Positions(26, 1),
            new Positions(25, 1),
            new Positions(24, 1),
            new Positions(23, 1),
            new Positions(22, 1),
            new Positions(21, 1),
            new Positions(20, 1),
            new Positions(19, 1),
            new Positions(18, 1),
            new Positions(17, 1),
            new Positions(16, 1),
            new Positions(15, 1),
            new Positions(15, 2),
            new Positions(15, 3),
            new Positions(15, 4),
            new Positions(16, 4),
            new Positions(17, 4),
            new Positions(18, 4),
            new Positions(18, 5),
            new Positions(18, 6),
        };
    }
}

