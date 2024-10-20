using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilson : Maze
{
   List<MapLocation> directions = new List<MapLocation>() {
 new MapLocation(1, 0),
        new MapLocation(0, 1),
        new MapLocation(-1, 0),
        new MapLocation(0, -1)
    };
    List<MapLocation> notUsed = new List<MapLocation>();
    public override void Generate()
    {
        int x = Random.Range(2, width - 1);
        int z = Random.Range(2, depth - 1);
        Map[x, z] = 2;
        while(GetAvailableCells()>1)
            RandomWalk();
        
    }
    int CountSquareMazeNeighbours(int x, int z)
    {

        int count = 0;
        for (int d = 0; d < directions.Count; d++)
        {
            int nx = x + directions[d].x;
            int nz = z + directions[d].z;
            if (Map[nx, nz] == 2)
            {
                count++;
            }

        }
        return count;
    }
    int GetAvailableCells()
    { 
        notUsed.Clear();
        for(int z = 1;z<depth-1;z++)
            for (int x = 1; x < width - 1; x++)
            {
                if (CountSquareNeighbours(x, z) ==0)
                {
                    notUsed.Add(new MapLocation(x, z));
                }
            }
        return notUsed.Count;
    }
    void RandomWalk()
    {
        List<MapLocation> inWalk = new List<MapLocation>();
        int cx ;
        int cz ;
        int rstartIndex=Random.Range(0,notUsed.Count);
        cx = notUsed[rstartIndex].x;
        cz = notUsed[rstartIndex].z; 
        inWalk.Add(new MapLocation(cx, cz));
        int loop = 0;
        bool validPath = false;
        while (cx > 0 && cx < width - 1 && cz > 0 && cz < depth - 1 && loop < 5000 && !validPath)
        {

            Map[cx, cz] = 0;
            if (CountSquareMazeNeighbours(cx, cz) > 1)
                break;
            int rd = Random.Range(0, directions.Count);
            int nx = cx + directions[rd].x;
            int nz = cz + directions[rd].z;
            if (CountSquareNeighbours(nx, nz) < 2)
            {
                cx = nx;
                cz = nz;
                inWalk.Add(new MapLocation(cx, cz));
            }
            validPath = CountSquareMazeNeighbours(cx, cz) == 1;
            loop++;
        }
        if (validPath)
        {
            Map[cx, cz] = 0;
            Debug.Log("PathFound");

            foreach (MapLocation m in inWalk)
            {
                Map[m.x, m.z] = 2;
            }
            inWalk.Clear();

        }
        else
        {
            foreach (MapLocation m in inWalk)
                Map[m.x, m.z] = 1;
            inWalk.Clear();
        }
    }
}
