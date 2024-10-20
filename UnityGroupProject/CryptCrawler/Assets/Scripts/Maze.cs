using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a class 'MapLocation' to represent coordinates in the maze
public class MapLocation
{
    public int x; // X coordinate
    public int z; // Z coordinate

    // Constructor to initialize the coordinates
    public MapLocation(int _x, int _z)
    {
        x = _x; // Set the x coordinate
        z = _z; // Set the z coordinate
    }
}

// Define a class 'Maze' that inherits from MonoBehaviour
public class Maze : MonoBehaviour
{
    // Public GameObject variables for maze components
    public GameObject cube; // Represents a wall cube
    public int width; // Width of the maze (x length)
    public int depth; // Depth of the maze (z length)
    public byte[,] Map; // 2D array to represent the maze structure
    public int scale; // Scale for the maze components

    // GameObjects for different maze segments
    public GameObject straight; // Straight path segment
    public GameObject crossroad; // Crossroad segment
    public GameObject corner; // Corner segment
    public GameObject tIntersection; // T intersection segment
    public GameObject endpiece; // End piece segment
    public GameObject FPC; // First Person Character or player object


    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap(); // Initialize the maze map
        Generate(); // Generate the maze layout
        DrawMap(); // Draw the maze in the scene
        PlaceFPC(); // Place the First Person Character in the maze
        //SpawnObjects(); // Uncomment to spawn objects in the maze (commented out for now)
    }

    // Initialize the maze map by setting all cells to walls (1)
    void InitialiseMap()
    {
        Map = new byte[width, depth]; // Create a new 2D array for the maze
        for (int z = 0; z < depth; z++) // Loop through each row
        {
            for (int x = 0; x < width; x++) // Loop through each column
            {
                Map[x, z] = 1;  // Set each cell to 1 (wall)
            }
        }
    }

    // Method to place the First Person Character (FPC) in the maze
    public virtual void PlaceFPC()
    {
        for (int z = 0; z < depth; z++) // Loop through each row
            for (int x = 0; x < width; x++) // Loop through each column
            {
                if (Map[x, z] == 0) // Check if the cell is a corridor (0)
                {
                    FPC.transform.position = new Vector3(x * scale, 0, z * scale); // Set FPC position
                    return; // Exit after placing FPC
                }
            }
    }




    // Virtual method to generate the maze layout
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++) // Loop through each row
            for (int x = 0; x < width; x++) // Loop through each column
            {
                // Randomly decide whether to make a cell a corridor (0) or a wall (1)
                if (UnityEngine.Random.Range(0, 100) < 50)
                    Map[x, z] = 0;  // Set the cell to 0 (corridor)
            }
    }

    void DrawMap()
    {
        // Loop through each layer of the map based on depth
        for (int z = 0; z < depth; z++)
            // Loop through each column of the map based on width
            for (int x = 0; x < width; x++)
            {
                // Check if the current position on the map is a wall (indicated by value 1)
                if (Map[x, z] == 1)
                {
                    // Uncomment below lines to create a wall object at the specified position
                    // Vector3 pos = new Vector3(x * scale, 0, z * scale); // Calculate the position based on scale
                    // GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a cube primitive for the wall
                    // wall.transform.localScale = new Vector3(scale, scale, scale); // Scale the wall to the desired size
                    // wall.transform.position = pos; // Set the wall's position in the scene
                }
                // Check for specific patterns in the map to place end pieces
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) // Horizontal end piece -|
                {
                    GameObject block = Instantiate(endpiece); // Instantiate the end piece object
                    block.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) // Horizontal end piece |-
                {
                    GameObject block = Instantiate(endpiece); // Instantiate the end piece object
                    block.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    block.transform.Rotate(0, 180, 0); // Rotate it 180 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) // Vertical end piece T
                {
                    GameObject block = Instantiate(endpiece); // Instantiate the end piece object
                    block.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    block.transform.Rotate(0, -90, 0); // Rotate it -90 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) // Vertical end piece upside down T
                {
                    GameObject block = Instantiate(endpiece); // Instantiate the end piece object
                    block.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    block.transform.Rotate(0, 90, 0); // Rotate it 90 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) // Vertical straight
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale); // Calculate position
                    Instantiate(straight, pos, Quaternion.identity); // Instantiate straight piece at position
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) // Horizontal straight
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale); // Calculate position
                    GameObject go = Instantiate(straight, pos, Quaternion.identity); // Instantiate straight piece
                    go.transform.Rotate(0, 90, 0); // Rotate it 90 degrees
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 })) // Crossroad
                {
                    GameObject go = Instantiate(crossroad); // Instantiate the crossroad piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                }
                // Check for different corner patterns and instantiate corner pieces accordingly
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 })) // Upper left corner
                {
                    GameObject go = Instantiate(corner); // Instantiate the corner piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    go.transform.Rotate(0, 180, 0); // Rotate it 180 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 })) // Upper right corner
                {
                    GameObject go = Instantiate(corner); // Instantiate the corner piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    go.transform.Rotate(0, 90, 0); // Rotate it 90 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 })) // Lower right corner
                {
                    GameObject go = Instantiate(corner); // Instantiate the corner piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 })) // Lower left corner
                {
                    GameObject go = Instantiate(corner); // Instantiate the corner piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    go.transform.Rotate(0, -90, 0); // Rotate it -90 degrees
                }
                // Check for different T-junction patterns and instantiate T-junction pieces accordingly
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 })) // T-junction upside down T
                {
                    GameObject go = Instantiate(tIntersection); // Instantiate the T-junction piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    go.transform.Rotate(0, -90, 0); // Rotate it -90 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 })) // T-junction T
                {
                    GameObject go = Instantiate(tIntersection); // Instantiate the T-junction piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    go.transform.Rotate(0, 90, 0); // Rotate it 90 degrees
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 })) // T-junction -|
                {
                    GameObject go = Instantiate(tIntersection); // Instantiate the T-junction piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                    go.transform.Rotate(0, 180, 0); // Rotate it 180 degrees
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 })) // T-junction |-
                {
                    GameObject go = Instantiate(tIntersection); // Instantiate the T-junction piece
                    go.transform.position = new Vector3(x * scale, 0, z * scale); // Set its position
                }
            }
    }
    //private void SpawnObjects()
    //{
    //    // Loop through each layer of the map based on depth
    //    for (int z = 0; z < depth; z++)
    //    {
    //        // Loop through each column of the map based on width
    //        for (int x = 0; x < width; x++)
    //        {
    //            // Check if the current position on the map is a corridor (indicated by value 0)
    //            if (Map[x, z] == 0) // Only spawn objects in corridors
    //            {
    //                // Generate a random number between 0 and 99
    //                // If the number is less than 10 (10% chance), spawn an enemy
    //                if (Random.Range(0, 100) < 10)
    //                {
    //                    // Instantiate an enemy prefab at the specified position
    //                    Instantiate(enemyPrefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    //                }

    //                // Generate a random number between 0 and 99
    //                // If the number is less than 5 (5% chance), spawn a lava trap
    //                if (Random.Range(0, 100) < 5)
    //                {
    //                    // Instantiate a lava trap prefab at the specified position
    //                    Instantiate(lavaTrapPrefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    //                }

    //                // Generate a random number between 0 and 99
    //                // If the number is less than 15 (15% chance), spawn an object spawner
    //                if (Random.Range(0, 100) < 15)
    //                {
    //                    // Instantiate an object spawner prefab at the specified position
    //                    Instantiate(objectSpawnerPrefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    //                }
    //            }
    //        }
    //    }
    //}

    // Function to search for a specific pattern in the 2D map
    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0; // Initialize a counter to keep track of matches
        int pos = 0; // Position index for the pattern array

        // Loop through a 3x3 grid centered at (c, r)
        for (int z = 1; z > -2; z--) // Iterate rows (from -1 to 1)
        {
            for (int x = -1; x < 2; x++) // Iterate columns (from -1 to 1)
            {
                // Check if the current map value matches the pattern or if the pattern value is 5 (wildcard)
                if (pattern[pos] == Map[c + x, r + z] || pattern[pos] == 5)
                    count++; // Increment the count if there's a match
                pos++; // Move to the next position in the pattern
            }
        }
        // Return true if all 9 positions matched, otherwise return false
        return (count == 9);
    }

    // Function to count the number of square neighbors (up, down, left, right) that are corridors (0)
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0; // Initialize a counter for neighbors
                       // Check boundaries; if out of bounds, return 5 (indicating potential neighbors)
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;

        // Check each of the four neighboring positions
        if (Map[x - 1, z] == 0) count++; // Left neighbor
        if (Map[x + 1, z] == 0) count++; // Right neighbor
        if (Map[x, z + 1] == 0) count++; // Up neighbor
        if (Map[x, z - 1] == 0) count++; // Down neighbor

        return count; // Return the total count of square neighbors
    }

    // Function to count the number of diagonal neighbors (top-left, top-right, bottom-left, bottom-right) that are corridors (0)
    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0; // Initialize a counter for diagonal neighbors
                       // Check boundaries; if out of bounds, return 5 (indicating potential neighbors)
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;

        // Check each of the four diagonal neighboring positions
        if (Map[x - 1, z - 1] == 0) count++; // Top-left neighbor
        if (Map[x + 1, z + 1] == 0) count++; // Bottom-right neighbor
        if (Map[x - 1, z + 1] == 0) count++; // Top-right neighbor
        if (Map[x + 1, z - 1] == 0) count++; // Bottom-left neighbor

        return count; // Return the total count of diagonal neighbors
    }

    // Function to count all neighbors (both square and diagonal)
    public int CountAllNeighbours(int x, int z)
    {
        // Sum the counts of square and diagonal neighbors and return the total
        return CountSquareNeighbours(x, z) + CountDiagonalNeighbours(x, z);
    }
}