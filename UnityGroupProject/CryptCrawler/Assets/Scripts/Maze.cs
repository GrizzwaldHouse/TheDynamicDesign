﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class Maze : MonoBehaviour
{
    // Declare four serialized GameObject variables: topWall, bottomWall, leftWall, and rightWall
    // These will be set in the Unity editor to specify the game objects that represent the walls of a maze cell
    public GameObject cube;
    public int width;//x legth
    public int depth;//z legth
    public byte[,] Map;
    public int scale;
    // Define a public void method called "Init" that takes four boolean parameters: top, bottom, right, and left
  

    public GameObject straight;
    public GameObject crossroad;
    public GameObject corner;
    public GameObject tIntersection;
    public GameObject endpiece;
    public GameObject FPC;
    //serialized fields for spawners and traps
    //[SerializeField] private GameObject enemyPrefab;
    //[SerializeField] private GameObject lavaTrapPrefab;
    //[SerializeField] private GameObject objectSpawnerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
        PlaceFPC();
       //SpawnObjects();
    }

    void InitialiseMap()
    {
        Map = new byte[width, depth];
        for(int z = 0;z <depth;z++)
        {
            for (int x = 0; x < width; x++)
            {
                    Map[x, z] = 1;  //1=wall 0= corridor
             
            }
    }
    }
    public virtual void PlaceFPC()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (Map[x, z] == 0)
                {
                    FPC.transform.position = new Vector3(x * scale, 0, z * scale);
                    return;
                }
            }
    }


    

    
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                    Map[x, z] = 0;  //0=corridor 1=wall
            }
    }
    void DrawMap()
    { 
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (Map[x, z] == 1)
                {
                    //Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    //GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //wall.transform.localScale = new Vector3(scale, scale, scale);
                    // wall.transform.position = pos;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) //horizontal end piece -|
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) //horizontal end piece |-
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                    block.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) //vertical end piece T
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                    block.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) //vertical end piece upside downT
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                    block.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) //vertical straight
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    Instantiate(straight, pos, Quaternion.identity);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) //horizontal straight
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(straight, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 })) //crossroad
                {
                    GameObject go = Instantiate(crossroad);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 })) //upper left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 })) //upper right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 })) //lower right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 })) //lower left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 })) //tjunc  upsidedown T
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 })) //tjunc  T
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 })) //tjunc  -|
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 })) //tjunc  |-
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
            }
    }
    //private void SpawnObjects()
    //{
    //    // Example of spawning enemies and traps
    //    for (int z = 0; z < depth; z++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            if (Map[x, z] == 0) // Only spawn in corridors
    //            {
    //                if (Random.Range(0, 100) < 10) // 10% chance to spawn an enemy
    //                {
    //                    Instantiate(enemyPrefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    //                }
    //                if (Random.Range(0, 100) < 5) // 5% chance to spawn a lava trap
    //                {
    //                    Instantiate(lavaTrapPrefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    //                }
    //                if (Random.Range(0, 100) < 15) // 15% chance to spawn an object spawner
    //                {
    //                    Instantiate(objectSpawnerPrefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    //                }
    //            }
    //        }
    //    }
    //}

    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0;
        int pos = 0;
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == Map[c + x, r + z] || pattern[pos] == 5)
                    count++;
                pos++;
            }
        }
        return (count == 9);
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (Map[x - 1, z] == 0) count++;
        if (Map[x + 1, z] == 0) count++;
        if (Map[x, z + 1] == 0) count++;
        if (Map[x, z - 1] == 0) count++;
        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (Map[x - 1, z - 1] == 0) count++;
        if (Map[x + 1, z + 1] == 0) count++;
        if (Map[x - 1, z + 1] == 0) count++;
        if (Map[x + 1, z - 1] == 0) count++;
        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }
}
