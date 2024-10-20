using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prims : Maze
{
    // Start is called before the first frame update
    public override void Generate()
    {
        int x = 2;
        int z = 2;
        Map[x, z] = 0;
        List<MapLocation> walls = new List<MapLocation>();
        walls.Add(new MapLocation(x+1,z));
        walls.Add(new MapLocation(x-1,z));
        walls.Add(new MapLocation(x,z+1));      
        walls.Add(new MapLocation(x,z-1));
        int countLoops = 0;
        while (walls.Count > 0 && countLoops < 5000)
        {
            int rwall = Random.Range(0, walls.Count);
            x=walls[rwall].x;
            z=walls[rwall].z;
            walls.RemoveAt(rwall);
            if (CountSquareNeighbours(x, z) == 1)
            {
                Map[x, z] = 0;
                walls.Add(new MapLocation(x + 1, z));
                walls.Add(new MapLocation(x - 1, z));
                walls.Add(new MapLocation(x, z + 1));
                walls.Add(new MapLocation(x, z - 1));
            }
            countLoops++;
        }
    }
}
