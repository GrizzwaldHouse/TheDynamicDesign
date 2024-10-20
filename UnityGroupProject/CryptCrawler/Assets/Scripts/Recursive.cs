using System.Collections; // Importing the Collections namespace for using collection types
using System.Collections.Generic; // Importing the Generic Collections namespace for using generic collection types
using Unity.Mathematics; // Importing Unity.Mathematics for mathematical operations
using UnityEngine; // Importing UnityEngine for Unity specific classes and functions

// Define a class 'Recursive' that inherits from 'Maze'
public class Recursive : Maze
{
    // List of possible movement directions (right, up, left, down)
    List<MapLocation> directions = new List<MapLocation>() {
        new MapLocation(1, 0),   // Move right
        new MapLocation(0, 1),   // Move up
        new MapLocation(-1, 0),  // Move left
        new MapLocation(0, -1)   // Move down
    };

    // String to store results (not used in the current implementation)
    string results;

    // Override the Generate method from the Maze class
    public override void Generate()
    {
        // Start the maze generation process from the coordinates (5, 5)
        Generate(5, 5);
    }

    // Recursive method to generate the maze
    void Generate(int x, int z)
    {
        // If the current cell has 2 or more neighboring cells, return (stop recursion)
        if (CountSquareNeighbours(x, z) >= 2) return;

        // Mark the current cell as part of the path (0)
        Map[x, z] = 0;

        // Shuffle the directions to ensure random path generation
        directions.Shuffle();

        // Recursively generate paths from the current cell in each of the shuffled directions
        Generate(x + directions[0].x, z + directions[0].z); // Move in the first shuffled direction
        Generate(x + directions[1].x, z + directions[1].z); // Move in the second shuffled direction
        Generate(x + directions[2].x, z + directions[2].z); // Move in the third shuffled direction
        Generate(x + directions[3].x, z + directions[3].z); // Move in the fourth shuffled direction
    }
}