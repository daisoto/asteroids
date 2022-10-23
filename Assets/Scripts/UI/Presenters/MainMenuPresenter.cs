using System;
using UnityEngine;
using Data;

namespace UI
{
public class MainMenuPresenter: Presenter<MainMenuView>
{
    private readonly SpaceshipDataManager _spaceshipDataManager;
    
    public MainMenuPresenter(MainMenuView view, 
        SpaceshipDataManager spaceshipDataManager): base (view)
    {
        _spaceshipDataManager = spaceshipDataManager;
    }
    
    public MainMenuPresenter Initialize(Action onNewGame, Action onContinue)
    {
        _view
            .OnNewGame(onNewGame)
            .OnContinue(onContinue)
            .SetContinue(CheckSave())
            .OnExit(ExitGame);
        
        return this;
    }
    
    public void Show() => _view.Show();
    
    private bool CheckSave() => _spaceshipDataManager.TryLoad(out var data);
    
    private void ExitGame() => Application.Quit();
}
}