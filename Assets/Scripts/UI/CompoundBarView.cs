using System.Collections.Generic;
using UnityEngine;

public class CompoundBarView: View
{
    [SerializeField]
    private GameObject _cellPrefab;
    
    [SerializeField]
    private Transform _cellsContainer;
    
    private IList<GameObject> _cells;
    
    public void Init(int maxNum)
    {
        _cells = new List<GameObject>();
        
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