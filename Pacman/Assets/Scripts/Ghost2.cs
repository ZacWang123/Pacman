using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Ghost2 : Ghost
{
    public Positions[] scatterPos;
    public Ghost2()
    {
        ghostPosition = new Positions(11, 16);
        Id = 5;
        scatterPos = new Positions[18] {
            new Positions(1, 29),
            new Positions(1, 28),
            new Positions(1, 27),
            new Positions(1, 26),
            new Positions(1, 25),
            new Positions(2, 25),
            new Positions(3, 25),
            new Positions(4, 25),
            new Positions(5, 25),
            new Positions(6, 25),
            new Positions(6, 26),
            new Positions(6, 27),
            new Positions(6, 28),
            new Positions(6, 29),
            new Positions(5, 29),
            new Positions(4, 29),
            new Positions(3, 29),
            new Positions(2, 29),
        };
    }

    public Positions[] NextPosition(Positions PacmanPosition) {
        Positions[] nextPosition = new Positions[4] {
            new Positions(0, 0),
            new Positions(0, 0),
            new Positions(0, 0),
            new Positions(0, 0)
        };

        if (this.mode == "Scatter") {
            for (int i = 0; i < scatterPos.Length; i++) {
                if (ghostPosition.Row == scatterPos[i].Row && ghostPosition.Col == scatterPos[i].Col) {
                    nextPosition[0] = scatterPos[(i+1)%18];
                    return nextPosition;
                }
            }

            nextPosition[0] = new Positions(ghostPosition.Row, ghostPosition.Col += 1);
            nextPosition[1] = new Positions(ghostPosition.Row -= 1, ghostPosition.Col);
            nextPosition[2] = new Positions(ghostPosition.Row, ghostPosition.Col -= 1);
            nextPosition[3] = new Positions(ghostPosition.Row += 1, ghostPosition.Col);
        }
        return nextPosition;
    }
}
