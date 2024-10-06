using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Define a public class called "MazeGenerator" that inherits from MonoBehaviour
public class MazeGenerator : MonoBehaviour
{
    // Declare two public integer variables with a range of 50 to 500: mazeWidth and mazeHeight
    // These will be set in the Unity editor to specify the dimensions of the maze
    [Range(50, 500)]
    public int mazeWidth,
     mazeHeight;

    // Declare two public integer variables: startX and startY
    // These will be set in the Unity editor to specify the starting position of the maze generation algorithm
    public int startX, startY;

    // Declare a 2D array of MazeCell objects: maze
    // This will represent the maze grid
    MazeCell[,] maze;

    // Declare a Vector2Int variable: currentCell
    // This will keep track of the current cell being processed by the maze generation algorithm
    Vector2Int currentCell;

    // Define a public method called "GetMaze" that returns a 2D array of MazeCell objects
    public MazeCell[,] GetMaze()
    {
        // Initialize the maze array with the specified width and height
        maze = new MazeCell[mazeWidth, mazeHeight];

        // Loop through each cell in the maze grid
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                // Create a new MazeCell object for each cell and initialize its position
                maze[x, y] = new MazeCell(x, y);
            }
        }

        // Start the maze generation algorithm from the specified starting position
        CarvePath(startX, startY);

        // Return the generated maze
        return maze;
    }

    // Define a list of Direction objects: directions
    List<Direction> directions = new List<Direction>()
    {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    // Define a method called "GetRandomDirections" that returns a list of randomly shuffled Direction objects
    List<Direction> GetRandomDirections()
    {
        // Make a copy of the directions list that can be used
        List<Direction> dir = new List<Direction>(directions);

        // Create a new list to store the randomly shuffled directions
        List<Direction> randomDirections = new List<Direction>();

        // Loop until all directions have been shuffled
        while (dir.Count > 0)
        {
            // Get a random direction from the list
            int rnd = Random.Range(0, dir.Count);

            // Add the random direction to the shuffled list
            randomDirections.Add(dir[rnd]);

            // Remove the random direction from the original list
            dir.RemoveAt(rnd);
        }

        // Return the shuffled list of directions
        return randomDirections;
    }

    // Define a method called "IsCellValid" that checks if a cell is within the maze boundaries and has not been visited
    bool IsCellValid(int x, int y)
    {
        // Check if the cell is within the maze boundaries
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
            return false;

        // Check if the cell has already been visited
        if (maze[x, y].visited)
            return false;

        // If the cell is valid, return true
        return true;
    }

    // Define a method called "CheckNeighbours" that checks the neighboring cells of the current cell
    Vector2Int CheckNeighbours()
    {
        // Get a list of randomly shuffled directions
        List<Direction> rndDir = GetRandomDirections();

        // Loop through each direction
        for (int i = 0; i < GetRandomDirections().Count; i++)
        {
            // Set the neighbor's coordinates to the current cell for now
            Vector2Int neighbour = currentCell;

            // Update the neighbor's coordinates based on the current direction
            switch (rndDir[i])
            {
                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
            }

            // Check if the neighbor is a valid cell
            if (IsCellValid(neighbour.x, neighbour.y))
                return neighbour;
        }

        // If no valid neighbor is found, return the current cell
        return currentCell;
    }

    // Define a method called "BreakWalls" that breaks the walls between two cells
    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        // Check which wall to break based on the direction of the secondary cell
        if (primaryCell.x > secondaryCell.x)
        {
            // Break the left wall of the primary cell
            maze[primaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.x < secondaryCell.x)
        {
            // Break the left wall of the secondary cell
            maze[secondaryCell.x, primaryCell.y].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y)
        {
            // Break the top wall of the primary cell
            maze[primaryCell.x, secondaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y)
        {
            // Break the top wall of the secondary cell
            maze[secondaryCell.x, primaryCell.y].topWall = false;
        }
    }

    // Define a method called "CarvePath" that carves a path through the maze
    void CarvePath(int x, int y)
    {
        // Check if the starting position is within the maze boundaries
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
        {
            // If not, default to the top-left corner
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0,0");
        }

        // Set the current cell to the starting position
        currentCell = new Vector2Int(x, y);

        // Create a list to store the path
        List<Vector2Int> path = new List<Vector2Int>();

        // Loop until a dead end is reached
        bool deadEnd = false;
        while (!deadEnd)
        {
            // Check the neighboring cells
            Vector2Int nextCell = CheckNeighbours();

            // If no valid neighbor is found, backtrack
            if (nextCell == currentCell)
            {
                // Loop through the path in reverse
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    // Set the current cell to the previous cell in the path
                    currentCell = path[i];

                    // Remove the previous cell from the path
                    path.RemoveAt(i);

                    // Check the neighboring cells again
                    nextCell = CheckNeighbours();

                    // If a valid neighbor is found, break the loop
                    if (nextCell != currentCell)
                        break;
                }

                // If no valid neighbor is found after backtracking, it's a dead end
                if (nextCell == currentCell)
                    deadEnd = true;
            }
            else
            {
                // Break the walls between the current cell and the next cell
                BreakWalls(currentCell, nextCell);

                // Mark the current cell as visited
                maze[currentCell.x, currentCell.y].visited = true;

                // Set the current cell to the next cell
                currentCell = nextCell;

                // Add the next cell to the path
                path.Add(currentCell);
            }
        }
    }

    // Define an enum called "Direction" to represent the four directions
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // Define a class called "MazeCell" to represent a cell in the maze
    public class MazeCell
    {
        // Declare a boolean variable to track whether the cell has been visited
        public bool visited;

        // Declare integer variables to store the cell's coordinates
        public int x, y;

        // Declare boolean variables to track the presence of walls
        public bool topWall;
        public bool leftWall;

        // Define a property called "position" to return the cell's coordinates as a Vector2Int
        public Vector2Int position
        {
            get
            {
                return new Vector2Int(x, y);
            }
        }

        // Define a constructor to initialize the cell's coordinates and walls
        public MazeCell(int x, int y)
        {
            // Initialize the cell's coordinates
            this.x = x;
            this.y = y;

            // Initialize the cell's walls
            topWall = leftWall = true;

            // Initialize the visited flag
            visited = false;
        }
    }
}