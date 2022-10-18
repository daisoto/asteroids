using System.Collections.Generic;

public static class RandomUtility
{
    private static readonly System.Random R = new System.Random(42);
    
    /// <summary>
    /// Includes both borders
    /// </summary>
    public static float GetFloat(float min, float max)
    {
        return (float)R.NextDouble() * (max - min) + min;
    }
    
    /// <summary>
    /// Includes both borders
    /// </summary>
    public static int GetInt(int min, int max)
    {
        return R.Next(min, max + 1);
    }
    
    public static T PickRandom<T>(this IList<T> source)
    {
        int index = R.Next(source.Count);
        
        return source[index];
    }
}