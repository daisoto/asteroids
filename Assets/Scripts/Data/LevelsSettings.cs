using UnityEngine;

namespace Data
{
[CreateAssetMenu(fileName = "New LevelsData", menuName = "Levels data")]
public class LevelsSettings: ScriptableObject
{
    [SerializeField]
    private int _maxLevel;
    public int MaxLevel => _maxLevel;
    
    [Header("Declares dependency from level")]
    [SerializeField]
    private AnimationCurve _spawnDelayCurve;
    
    [SerializeField]
    private Color _openedColor;
    public Color OpenedColor => _openedColor;
    
    [SerializeField]
    private Color _closedColor;
    public Color ClosedColor => _closedColor;
    
    [SerializeField]
    private Color _finishedColor;
    public Color FinishedColor => _finishedColor;
    
    public float GetDelay(int level)
    {
        var ratio = level / _maxLevel;
        
        return _spawnDelayCurve.Evaluate(ratio);
    }
}
}