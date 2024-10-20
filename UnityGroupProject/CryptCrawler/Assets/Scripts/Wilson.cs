using System.Collections; // Importing the Collections namespace for using List
using System.Collections.Generic; // Importing the Generic Collections namespace for using List<T>
using UnityEngine; // Importing UnityEngine for Unity specific classes and functions

// Define a class 'Wilson' that inherits from 'Maze'
public class Wilson : Maze
{
    // List of possible movement directions (right, up, left, down)
    List<MapLocation> directions = new List<MapLocation>() {
        new MapLocation(1, 0),   // Move right
        new MapLocation(0, 1),   // Move up
        new MapLocation(-1, 0),  // Move left
        new MapLocation(0, -1)   // Move down
    };

    // List to keep track of unused cells
    List<MapLocation> notUsed = new List<MapLocation>();

    // Override the Generate method from the Maze class
    public override void Generate()
    {
        // Randomly select starting coordinates within the maze bounds
        int x = Random.Range(2, width - 1);
        int z = Random.Range(2, depth - 1);

        // Mark the starting cell as part of the maze (2)
        Map[x, z] = 2;

        // Continue random walking until only one available cell remains
        while (GetAvailableCells() > 1)
            RandomWalk(); // Call the RandomWalk method to carve paths
    }

    // Count the number of neighboring cells that are part of the maze (marked as 2)
    int CountSquareMazeNeighbours(int x, int z)
    {
        int count = 0; // Initialize a count of neighboring cells
        // Iterate through each direction to check neighbors
        for (int d = 0; d < directions.Count; d++)
        {
            int nx = x + directions[d].x; // Calculate new x coordinate
            int nz = z + directions[d].z; // Calculate new z coordinate
            // Check if the neighboring cell is part of the maze
            if (Map[nx, nz] == 2)
            {
                count++; // Increment count if it is part of the maze
            }
        }
        return count; // Return the total count of neighboring maze cells
    }

    // Get the count of cells that are available for path generation
    int GetAvailableCells()
    {
        notUsed.Clear(); // Clear the list of unused cells
        // Loop through the maze to find cells with no neighboring maze cells
        for (int z = 1; z < depth - 1; z++)
            for (int x = 1; x < width - 1; x++)
            {
                // If a cell has no neighboring maze cells, add it to notUsed list
                if (CountSquareNeighbours(x, z) == 0)
                {
                    notUsed.Add(new MapLocation(x, z)); // Add unused cell location
                }
            }
        return notUsed.Count; // Return the count of available cells
    }

    // Method to perform a random walk to carve out paths in the maze
    void RandomWalk()
    {
        List<MapLocation> inWalk = new List<MapLocation>(); // List to track the current walk path
        int cx; // Current x position
        int cz; // Current z position
        int rstartIndex = Random.Range(0, notUsed.Count); // Randomly select a starting position from notUsed
        cx = notUsed[rstartIndex].x; // Set current x position
        cz = notUsed[rstartIndex].z; // Set current z position
        inWalk.Add(new MapLocation(cx, cz)); // Add starting location to the walk path

        int loop = 0; // Loop counter to prevent infinite loops
        bool validPath = false; // Flag to determine if a valid path is found

        // Continue walking while within bounds and loop limit is not exceeded
        while (cx > 0 && cx < width - 1 && cz > 0 && cz < depth - 1 && loop < 5000 && !validPath)
        {
            Map[cx, cz] = 0; // Mark the current cell as part of the path (0)
            // Check if the current cell has more than one neighbor
            if (CountSquareMazeNeighbours(cx, cz) > 1)
                break; // Exit the loop if more than one neighbor is found

            int rd = Random.Range(0, directions.Count); // Randomly select a direction to move
            int nx = cx + directions[rd].x; // Calculate new x position
            int nz = cz + directions[rd].z; // Calculate new z position

            // Ensure the next cell is not already part of the maze
            if (CountSquareNeighbours(nx, nz) < 2)
            {
                cx = nx; // Update current x position
                cz = nz; // Update current z position
                inWalk.Add(new MapLocation(cx, cz)); // Add new position to the walk path
            }

            // Check if the current cell has only one neighbor (a valid path)
            validPath = CountSquareMazeNeighbours(cx, cz) == 1;
            loop++; // Increment loop counter
        }

        // If a valid path is found, mark the entire path as part of the maze
        if (validPath)
        {
            Map[cx, cz] = 0; // Mark the final cell as part of the path
            Debug.Log("PathFound"); // Log a message indicating a path was found

            // Mark the entire walk path as part of the maze
            foreach (MapLocation m in inWalk)
            {
                Map[m.x, m.z] = 2; // Mark each cell as part of the maze
            }
            inWalk.Clear(); // Clear the walk path list
        }
        else
        {
            // If no valid path is found, mark the entire walk path as walls
            foreach (MapLocation m in inWalk)
                Map[m.x, m.z] = 1; // Mark each cell as a wall
            inWalk.Clear(); // Clear the walk path list
        }
    }
}