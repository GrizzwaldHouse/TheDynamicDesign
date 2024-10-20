using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Recursive : Maze
{
    List<MapLocation> directions = new List<MapLocation>() {
 new MapLocation(1, 0),
        new MapLocation(0, 1),
        new MapLocation(-1, 0),
        new MapLocation(0, -1)
    };
    string results;
    public override void Generate()
    {
        Generate(5, 5);
    }

void Generate(int x, int z)
{
    if (CountSquareNeighbours(x, z) >= 2) return;
    Map[x, z] = 0;
        directions.Shuffle();
        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x+ directions[2].x, z + directions[2].z);
        Generate(x+ directions[2].x, z + directions[3].z);
    }
}
