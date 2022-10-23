using UnityEngine;

namespace Data
{
[CreateAssetMenu(fileName = "New LevelsData", menuName = "Levels data")]
public class LevelsData: ScriptableObject, ILevelsViewDataProvider
{
    [SerializeField]
    private int _maxLevel;
    public int MaxLevel => _maxLevel;
    
    [SerializeField]
    private Color _openedColor;
    public Color OpenedColor => _openedColor;
    
    [SerializeField]
    private Color _closedColor;
    public Color ClosedColor => _closedColor;
    
    [SerializeField]
    private Color _finishedColor;
    public Color FinishedColor => _finishedColor;
}
}