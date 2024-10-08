using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MazeGeneratorLib;
public class GenerateMaze : MonoBehaviour {

    
    public enum MazeGeneratorMode { WithPrefabs, WihtoutPrefabs}
    public Vector3 startPos = new Vector3(0, 0, 0);
    public MazeGeneratorMode MazeMode = MazeGeneratorMode.WithPrefabs;

    public GameObject wallPrefab;
    public int MazeWidth = 10, MazeHeight = 10;
    public float TunnelWidth = 1;
    public float WallWidth = 1;        

    public bool IsCreateGround = true;
    public GameObject groundPrefab;
    public bool IsCreateCeiling = false;
    public GameObject ceilingPrefab;

    public bool IsCreateMazeOnStart = true; 

    public bool UseStartEndWall = false;
    public GameObject StartWallPrefab, EndWallPrefab;   
      
    private GameObject endWallobj, startWallobj;
    private GameObject groundObg, ceilingObj;
    private List<GameObject> mazeWalls;    
    private List<Vector3> wallPosArray, startWallPos, endWallPos;
    
    private MazeSkeleton maze;
    void Start () {
        
        wallPosArray = new List<Vector3>();
        endWallPos = new List<Vector3>();
        startWallPos = new List<Vector3>();
        mazeWalls = new List<GameObject>();
        maze = new MazeSkeleton(new Vector3(0, 0, 0),MazeWidth,MazeHeight,TunnelWidth,WallWidth,1,IsCreateMazeOnStart);     
                
        if(IsCreateMazeOnStart) DrawMaze();        

    }
    private bool IsPosInArray(Vector3 pos, List<Vector3> array)
    {
        bool toRet = false;
        for (int i = 0; i < array.Count; i++)
        {
            if (pos == array[i])
            {
                toRet = true;
                break;
            }

        }
        return toRet;
    }
    private void DrawMaze()
    {
        wallPosArray = maze.GetWallArray();        
        endWallPos = maze.GetEndWallArray();
        startWallPos = maze.GetStartWallArray();        
        
        if (MazeMode == MazeGeneratorMode.WithPrefabs)
        {
            for (int j = 0; j < wallPosArray.Count; j++)
            {
                
                if (UseStartEndWall)
                {

                    if (IsPosInArray(wallPosArray[j], endWallPos) && EndWallPrefab != null)
                    {

                        GameObject obj = (GameObject)Instantiate(EndWallPrefab, wallPosArray[j], Quaternion.identity);
                        obj.transform.parent = transform;
                        mazeWalls.Add(obj);
                    }
                    else if (IsPosInArray(wallPosArray[j], startWallPos) && StartWallPrefab!=null)
                    {

                        GameObject obj = (GameObject)Instantiate(StartWallPrefab, wallPosArray[j], Quaternion.identity);
                        obj.transform.parent = transform;
                        mazeWalls.Add(obj);
                    }
                    else if(wallPrefab != null)
                    {
                        GameObject obj = (GameObject)Instantiate(wallPrefab, wallPosArray[j], Quaternion.identity);
                        obj.transform.parent = transform;
                        mazeWalls.Add(obj);
                    }
                }
                else if(wallPrefab != null)
                {
                    GameObject obj = (GameObject)Instantiate(wallPrefab, wallPosArray[j], Quaternion.identity);
                    obj.transform.parent = transform;
                    mazeWalls.Add(obj);
                }
            }
            
        }
        if (IsCreateGround && groundPrefab!=null)
        {
            groundObg = (GameObject)Instantiate(groundPrefab, maze.GetPosionForGround(), Quaternion.identity);
            groundObg.transform.localScale = new Vector3(maze.GetSizeForGround().x, 1, maze.GetSizeForGround().y);
            groundObg.transform.parent = transform;
            mazeWalls.Add(groundObg);

        }

        if (IsCreateCeiling && ceilingPrefab!=null)
        {
            ceilingObj = (GameObject)Instantiate(ceilingPrefab, maze.GetPosionForCeiling(), Quaternion.identity);
            ceilingObj.transform.localScale = new Vector3(maze.GetSizeForGround().x, 1, maze.GetSizeForGround().y);
            ceilingObj.transform.parent = transform;
            mazeWalls.Add(ceilingObj);
        }
    }
    private void UnDrawMaze()
    {
        for(int i=0;i< mazeWalls.Count;i++)
        {
            Destroy(mazeWalls[i]);
        }
        mazeWalls.Clear();
    }
    public void DeleteMaze()
    {
        UnDrawMaze();
        maze.DeleteMaze();
    }
    public void SetCreateCeiling(bool create)
    {
        IsCreateCeiling = create;
    }
    public void SetWallType(GameObject newWall)
    {
        wallPrefab = newWall;
    }
    
    public void SetGroundPrefab(GameObject newGround)
    {
        groundPrefab = newGround;
    }
    public void SetCeilingPrefab(GameObject newGround)
    {
        ceilingPrefab = newGround;
    }
    public void SetStartWallPrefab(GameObject newGround)
    {
        StartWallPrefab = newGround;
    }
    public void SetStopWallPrefab(GameObject newGround)
    {
        EndWallPrefab = newGround;
    }
    public Vector2 GetMazeSizeInCells()
    {
        return new Vector2(MazeWidth, MazeHeight);
    }
   
    public void ClearAndGenerateNewMaze(int newMazeWidth, int newMazeHeight)
    {
        UnDrawMaze();
        maze.DeleteMaze();
        MazeWidth = newMazeWidth;
        MazeHeight = newMazeHeight;
        Invoke("GenMaze", 0.5f);
    }    
    
    private void GenMaze()
    {
        if(maze.IsGenerated())
        {
            UnDrawMaze();
            maze.DeleteMaze();
        }        
        maze.SetMazeDepth(1);       
        maze.GenerateNewMaze(MazeWidth, MazeHeight);
        DrawMaze();
    }
        
    public void GenerateNewMaze(int newMazeWidth, int newMazeHeight)
    {
        MazeWidth = newMazeWidth;
        MazeHeight = newMazeHeight;
        GenMaze();
    }    
    public Vector3[,] GetGridCenters()
    {
        return maze.GetGridCenters();
    }  
    
    public List<Vector3> GetWallArray()
    {
        return maze.GetWallArray();
    }
    public Vector2 GetSizeForGround()
    {
        return maze.GetSizeForGround();
    }
   
}

