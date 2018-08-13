using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WeightedRandomizer<T>
{
    public Dictionary<T, int> Weights;

    /// <summary>
    /// Instead of calling this constructor directly,
    /// consider calling a static method on the WeightedRandomizer (non-generic) class
    /// for a more readable method call, i.e.:
    /// 
    /// <code>
    /// var selected = WeightedRandomizer.From(weights).TakeOne();
    /// </code>
    /// 
    /// </summary>
    /// <param name="weights"></param>
    public WeightedRandomizer(Dictionary<T, int> weights)
    {
        Weights = weights;
    }

    public WeightedRandomizer()
    {
        Weights = new Dictionary<T, int>();
    }

    /// <summary>
    /// Randomizes one item
    /// </summary>
    /// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
    /// <returns>The randomized item.</returns>
    public T TakeOne()
    {
        // Sorts the spawn rate list
        var sortedSpawnRate = Sort(Weights);

        // Sums all spawn rates
        int sum = 0;
        foreach (var spawn in Weights)
        {
            sum += spawn.Value;
        }

        // Randomizes a number from Zero to Sum
        int roll = Random.Range(0, sum);

        // Finds chosen item based on spawn rate
        T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        // Returns the selected item
        return selected;
    }

    private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
    {
        var list = new List<KeyValuePair<T, int>>(weights);

        // Sorts the Spawn Rate List for randomization later
        list.Sort(
            delegate (KeyValuePair<T, int> firstPair,
                        KeyValuePair<T, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );

        return list;
    }
}
