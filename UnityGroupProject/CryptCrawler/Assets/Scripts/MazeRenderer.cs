using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MazeGenerator;

public class MazeRender : MonoBehaviour
{
    [SerializeField]MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;
    public float CellSize = 1f;
    private void Strat()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();
        for(int x =0; x < mazeGenerator.mazeWidth; x++)
        {
            for(int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity);
                MazeCellObject mazeCell= newCell.GetComponent<MazeCellObject>();
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                bool right = false;

                bool bottom = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if(y==0) bottom = true;
                mazeCell.Init(top, bottom, right, left);

            }
        }
    }
}
