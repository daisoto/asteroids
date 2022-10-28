using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class CompoundBarView: View
{
    [SerializeField]
    private GameObject _cellPrefab;
    
    [SerializeField]
    private RectTransform _cellsContainer;
    
    private readonly List<GameObject> _cells = new List<GameObject>();
    
    public void Construct(int maxNum)
    {
        var curCount = _cells.Count;
        if (curCount < maxNum)
        {
            var diff = maxNum - curCount;
            _cells.AddRange(new List<GameObject>(diff));
        }

        for (int i = 0; i < maxNum; i++)
        {
            var cell = Instantiate(_cellPrefab, _cellsContainer);
            _cells.Add(cell);
        }
    }
    
    public void SetCells(int num)
    {
        for (int i = 0; i < _cells.Count; i++)
            _cells[i].SetActive(i < num);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_cellsContainer);
    }
}
}