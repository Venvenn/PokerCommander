
using Random = Unity.Mathematics.Random;

public class SeededRandom
{
    public Random Random;

    /// <summary>
    /// Creates a Seeded Random with a random seed 
    /// </summary>
    public SeededRandom()
    {
        Random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue));
    }
    
    /// <summary>
    /// Creates a Seeded Random with a chosen seed
    /// </summary>
    public SeededRandom(uint seed)
    {
        Random = Random.CreateFromIndex(seed);
    }
}
