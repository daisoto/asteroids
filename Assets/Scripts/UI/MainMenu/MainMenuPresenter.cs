using System;
using UnityEngine;
using Data;
using Zenject;

namespace UI
{
public class MainMenuPresenter: Presenter<MainMenuView>, IInitializable
{
    private readonly SpaceshipDataManager _spaceshipDataManager;
    
    private Action _onContinue;
    private Action _onNewGame;
    
    public MainMenuPresenter(MainMenuView view, 
        SpaceshipDataManager spaceshipDataManager): base (view)
    {
        _spaceshipDataManager = spaceshipDataManager;
    }
    
    public void Initialize()
    {
        _view
            .OnNewGame(NewGame)
            .OnContinue(Continue)
            .SetContinue(CheckSave())
            .OnExit(ExitGame);
    }
    
    public MainMenuPresenter SetOnNewGame(Action onNewGame)
    {
        _onNewGame = onNewGame;
        
        return this;
    }
    
    public MainMenuPresenter SetOnContinue(Action onContinue)
    {
        _onContinue = onContinue;
        
        return this;
    }
    
    private void NewGame()
    {
        Close();
        _onNewGame?.Invoke();
    }
    
    private void Continue()
    {
        Close();
        _onContinue?.Invoke();
    }
    
    private bool CheckSave() => _spaceshipDataManager.TryLoad(out var data);
    
    private void ExitGame() => Application.Quit();
}
}