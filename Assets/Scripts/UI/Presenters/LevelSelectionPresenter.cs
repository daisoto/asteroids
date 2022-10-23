using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Gameplay;

namespace UI
{
public class LevelSelectionPresenter: Presenter<LevelSelectionView>
{
    private readonly ILevelsViewDataProvider _levelsViewDataProvider;
    private readonly LevelsController _levelsController;

    private Color _finishedColor => _levelsViewDataProvider.FinishedColor; 
    private Color _openedColor => _levelsViewDataProvider.OpenedColor; 
    private Color _closedColor => _levelsViewDataProvider.ClosedColor; 
    
    public LevelSelectionPresenter(LevelSelectionView view, 
        ILevelsViewDataProvider levelsViewDataProvider, 
        LevelsController levelsController): base(view)
    {
        _levelsViewDataProvider = levelsViewDataProvider;
        _levelsController = levelsController;
    }

    public override void Show()
    {
        base.Show();
        _view.Draw(GetViewModels(), StartLevel);
    }
    
    private void StartLevel(int level)
    {
        _levelsController.StartLevel(level, Show);
    }
    
    private IList<LevelButtonViewModel> GetViewModels()
    {
        var viewModels = new List<LevelButtonViewModel>();
        var data = _levelsController.LevelsData;
        var maxLevel = _levelsViewDataProvider.MaxLevel;
        
        for (int level = 1; level <= maxLevel; level++)
        {
            var isFirst = level == 1;
            var levelData = data
                .FirstOrDefault(d => d.Level == level);
            var prevLevelData = data
                .FirstOrDefault(d => d.Level == level - 1);
            
            var isFinished = levelData is {IsFinished: true};
            var isPrevFinished = prevLevelData is {IsFinished: true};
            
            var viewModel = new LevelButtonViewModel
            {
                Level = level,
                IsAvailable = isFirst || isPrevFinished,
                Color = isFinished ? _finishedColor : 
                    isFirst || isPrevFinished ? _openedColor : _closedColor
            };
            
            viewModels.Add(viewModel);
        }
        
        return viewModels;
    }
}
}