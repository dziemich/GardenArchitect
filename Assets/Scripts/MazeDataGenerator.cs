using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    // generator params
    public float placementThreshold; // chance of empty space

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];

        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                
             
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }

                // every other inside space
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        maze[i, j] = 1;

                        // in addition to this spot, randomly place adjacent
                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }

        maze[0, 0] = 0;
        maze[0, 1] = 0;
        maze[1, 0] = 0;
        maze[rMax, cMax] = 0;
        maze[rMax-1, cMax] = 0;
        maze[rMax, cMax-1] = 0;
        return maze;
    }
}