using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class MapLoction
{

    public int x;
    public int z;
    public MapLoction(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

// Define a public class called "MazeCellObject" that inherits from MonoBehaviour
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
    public void Start()
    {

        InitialiseMap();
        Generate();
        DrawMap();
      
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
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    Map[x, z] = 0;  //1=wall 0= corridor
                }
            }
    }
    void DrawMap()
    {
        for (int z=0; z < depth; z++)
            for (int x = 0;x < width; x++)
            {
                if (Map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
    }

    public int CountSquareNeighbours(int x,int z)
    {
        int count = 0;
        if (z <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5 ;
        if (Map[x - 1, z] == 0) count++;
        if (Map[x + 1, z] == 0) count++;
        if (Map[x, z + 1] == 0) count++;
        if (Map[x, z - 1] == 0) count++;
        return count;

    }
    public int CountDiagonalNeighbours(int x,int z)
    {
        int count=0;
        if (z <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (Map[x - 1, z-1] == 0) count++;
        if (Map[x + 1, z+1] == 0) count++;
        if (Map[x-1, z + 1] == 0) count++;
        if (Map[x+1, z - 1] == 0) count++;
        return count;
    }
    public int CountAllNeigbors(int x, int z)
    {
        return CountSquareNeighbours(x,z) +CountDiagonalNeighbours(x,z);
    }
}
