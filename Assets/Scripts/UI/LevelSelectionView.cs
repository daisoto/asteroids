using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView: View
{
    [SerializeField]
    private TextButton _levelPrefab;
    
    [SerializeField]
    private Transform _levelsContainer;
    
    [SerializeField]
    private Button _backButton;
    
    private List<TextButton> _levelButtons = new List<TextButton>();

    private void OnDestroy()
    {
        _backButton.onClick.RemoveAllListeners();
    }

    public LevelSelectionView Draw(
        IList<LevelButtonViewModel> viewModels, 
        Action<int> onSelect)
    {
        foreach (var viewModel in viewModels)
        {
            var level = viewModel.Level;
            var button = GetLevelButton(level)
                .SetColor(viewModel.Color);
            button.onClick.AddListener(() => onSelect.Invoke(level));
            button.interactable = viewModel.IsAvailable;
        }
        
        return this;
    }
    
    public LevelSelectionView SetOnBack(Action onBack)
    {
        _backButton.onClick.AddListener(onBack.Invoke);
        
        return this;
    }
    
    private TextButton GetLevelButton(int level)
    {
        if (_levelButtons.Count >= level)
        {
            var button = _levelButtons[level - 1];
            button.onClick.RemoveAllListeners();
            
            return button;
        }
        
        var newButton = Instantiate(_levelPrefab, _levelsContainer)
            .SetText(level.ToString());
        _levelButtons.Add(newButton);
        
        return newButton;
    }
}