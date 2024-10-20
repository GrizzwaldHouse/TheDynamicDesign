// Import necessary namespaces for collections and Unity functionality
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a class Prims that inherits from the Maze class
public class Prims : Maze
{
    // Override the Generate method to implement Prim's algorithm for maze generation
    public override void Generate()
    {
        // Initialize the starting point of the maze at coordinates (2, 2)
        int x = 2;
        int z = 2;

        // Mark the starting point as a visited cell (represented by 0)
        Map[x, z] = 0;

        // Create a list to store the wall locations that need to be processed
        List<MapLocation> walls = new List<MapLocation>();

        // Add the initial wall locations around the starting point to the list
        // These walls are the potential candidates for carving paths in the maze
        walls.Add(new MapLocation(x + 1, z)); // Right wall
        walls.Add(new MapLocation(x - 1, z)); // Left wall
        walls.Add(new MapLocation(x, z + 1)); // Top wall
        walls.Add(new MapLocation(x, z - 1)); // Bottom wall

        // Initialize a counter to track the number of iterations
        int countLoops = 0;

        // Continue processing walls until the list is empty or a maximum of 5000 iterations is reached
        while (walls.Count > 0 && countLoops < 5000)
        {
            // Randomly select a wall from the list
            int rwall = Random.Range(0, walls.Count);

            // Get the coordinates of the selected wall
            x = walls[rwall].x;
            z = walls[rwall].z;

            // Remove the selected wall from the list
            walls.RemoveAt(rwall);

            // Check if the selected wall has only one neighboring visited cell
            if (CountSquareNeighbours(x, z) == 1)
            {
                // Mark the wall as a visited cell (represented by 0)
                Map[x, z] = 0;

                // Add the neighboring walls of the newly visited cell to the list
                // These walls are the new potential candidates for carving paths in the maze
                walls.Add(new MapLocation(x + 1, z)); // Right wall
                walls.Add(new MapLocation(x - 1, z)); // Left wall
                walls.Add(new MapLocation(x, z + 1)); // Top wall
                walls.Add(new MapLocation(x, z - 1)); // Bottom wall
            }

            // Increment the iteration counter
            countLoops++;
        }
    }
}