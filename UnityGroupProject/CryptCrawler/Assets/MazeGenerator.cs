using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MazeGenerator : MonoBehaviour
{

    [SerializeField] private MazeCell _mazeCellPrefab;

    [SerializeField] int _mazeWidth;

    [SerializeField]  int _mazeDepth;

    private MazeCell [,] _mazeGrid;

    

    // Start is called before the first frame update
    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth,_mazeDepth];
        for(int x= 0; x < _mazeWidth ; x++)
        {
            for(int z = 0; z < _mazeDepth ; z++)
            {
                _mazeGrid[x,z] =Instantiate(_mazeCellPrefab,new Vector3(x,0,z),Quaternion.identity);
            }
        }
        GenerateMaze(null,_mazeGrid[0, 0]);
    }
    private void GenerateMaze(MazeCell prevCell, MazeCell currentCell)
    {
        currentCell.IsVisit();
        ClearWalls(prevCell, currentCell);
        
        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnivisitedCell(currentCell);
            if (nextCell != null)
            {

                GenerateMaze(currentCell, nextCell);

            }
        }
        while (nextCell != null);
        
    }
    private MazeCell GetNextUnivisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCell(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

    }
    private IEnumerable<MazeCell> GetUnvisitedCell (MazeCell currentCell)
    {
        
        int x=(int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;
        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }
        if(x -1 >=0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if(cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }
        if (z -1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z = 1];
            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if(cellToBack.isVisited== false)
            {
                yield return cellToBack;
            }
        }

    }

    void ClearWalls(MazeCell prevCell, MazeCell currentCell)
    {
        if (prevCell == null)
        {
            return;
        }
        if (prevCell.transform.position.x < currentCell.transform.position.x)
        {
            prevCell.clearRightWall();
            currentCell.clearLeftWall();
            return;
        }
        if (prevCell.transform.position.z < currentCell.transform.position.z)
        {
            prevCell.clearLeftWall();
            currentCell.clearBackWall();
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
