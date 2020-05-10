using System.Collections.Generic;
using System.Linq;

namespace FiveSquared
{
    public class Combinations
    {
        // Default constructor
        public Combinations()
        {
        
        }

        public List<List<T>> FindCombinations<T>(List<T> elements, int k) 
        {
            bool[] Selected = new bool[elements.Count];

            List<List<T>> results = new List<List<T>>();

            SelectItems(elements, Selected, results, k, 0);

            return results; 
        }

        public void SelectItems<T>(List<T> elements, bool[] selected, List<List<T>> results, int k, int start) 
        {
            if (k == 0) 
            {
                List<T> Selection = new List<T>();
                for (int i = 0; i < elements.Count; i++)
                {
                    if (selected[i]) Selection.Add(elements[i]);
                }
                results.Add(Selection);
            } else 
            {
                for (int i = start; i < elements.Count; i++)
                {
                    selected[i] = true; 
                    SelectItems(elements, selected, results, k - 1, i + 1);
                    selected[i] = false; 
                }
            }
        }

                // Generate permutations.
        public List<List<T>> GeneratePermutations<T>(List<T> items)
        {
            // Make an array to hold the
            // permutation we are building.
            T[] current_permutation = new T[items.Count];

            // Make an array to tell whether
            // an item is in the current selection.
            bool[] in_selection = new bool[items.Count];

            // Make a result list.
            List<List<T>> results = new List<List<T>>();

            // Build the combinations recursively.
            PermuteItems<T>(items, in_selection,
                current_permutation, results, 0);

            // Return the results.
            return results;
        }

        // Recursively permute the items that are
        // not yet in the current selection.
        public void PermuteItems<T>(List<T> items, bool[] in_selection,
            T[] current_permutation, List<List<T>> results,
            int next_position)
        {
            // See if all of the positions are filled.
            if (next_position == items.Count)
            {
                // All of the positioned are filled.
                // Save this permutation.
                results.Add(current_permutation.ToList());
            }
            else
            {
                // Try options for the next position.
                for (int i = 0; i < items.Count; i++)
                {
                    if (!in_selection[i])
                    {
                        // Add this item to the current permutation.
                        in_selection[i] = true;
                        current_permutation[next_position] = items[i];

                        // Recursively fill the remaining positions.
                        PermuteItems<T>(items, in_selection,
                            current_permutation, results,
                            next_position + 1);

                        // Remove the item from the current permutation.
                        in_selection[i] = false;
                    }
                }
            }
        }
    }
}