using System.Collections.Generic;

public static class RandomUtils
{
    private static readonly System.Random R = new System.Random(42);
    
    /// <summary>
    /// Includes both borders
    /// </summary>
    public static float GetFloat(float min, float max) =>
        (float)R.NextDouble() * (max - min) + min;
    
    /// <summary>
    /// Includes both borders
    /// </summary>
    public static int GetInt(int min, int max) =>
        R.Next(min, max + 1);
    
    /// <summary>
    /// <param name="prob"> must be from 0 to 1</param>
    /// </summary>
    public static bool ProcessProbability(double prob) =>
        R.NextDouble() < prob;
    
    public static T PickRandom<T>(this IList<T> source) => 
        source[R.Next(source.Count)];
}