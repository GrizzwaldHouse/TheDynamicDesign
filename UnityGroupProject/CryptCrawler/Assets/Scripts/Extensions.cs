using System.Collections; // Importing the Collections namespace for using collection types
using System.Collections.Generic; // Importing the Generic Collections namespace for using generic collection types
using UnityEngine; // Importing UnityEngine for Unity specific classes and functions

// Define a static class 'Extension' to hold extension methods
public static class Extension
{
    // Static instance of System.Random for random number generation
    public static System.Random rng = new System.Random();

    // Extension method to shuffle elements in a list of type T
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count; // Get the number of elements in the list
        // Continue shuffling until only one element remains
        while (n > 1)
        {
            n--; // Decrement the count of elements to consider
            int k = rng.Next(n + 1); // Generate a random index 'k' from 0 to n
            T value = list[k]; // Store the value at index 'k'
            list[k] = list[n]; // Swap the element at index 'k' with the element at index 'n'
            list[n] = value; // Assign the stored value to index 'n'
        }
    }
}
