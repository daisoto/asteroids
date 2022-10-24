using System.Collections.Generic;
using UnityEngine;

namespace UI
{
public class CompoundBarView: View
{
    [SerializeField]
    private GameObject _cellPrefab;
    
    [SerializeField]
    private Transform _cellsContainer;
    
    private List<GameObject> _cells;
    
    private void Start()
    {
        _cells = new List<GameObject>();
    }
    
    public void Init(int maxNum)
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
        var count = _cells.Count;
        
        if (num > count)
        {
            Debug.LogError("Required cells num is too big!");
            
            return;
        }
        
        for (int i = 0; i < count; i++)
            _cells[i].SetActive(i <= num);
    }
}
}