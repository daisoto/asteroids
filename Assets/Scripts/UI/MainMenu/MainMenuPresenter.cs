using System;
using Data;
using Gameplay;
using Zenject;

namespace UI
{
public class MainMenuPresenter: Presenter<MainMenuView>, IInitializable
{
    private readonly SpaceshipDataManager _spaceshipDataManager;
    private readonly SignalBus _signalBus;
    
    private SpaceshipData _loadedData;
    private Action _onContinue;
    private Action _onNewGame;
    
    public MainMenuPresenter(MainMenuView view, 
        SpaceshipDataManager spaceshipDataManager, 
        SignalBus signalBus): base (view)
    {
        _spaceshipDataManager = spaceshipDataManager;
        _signalBus = signalBus;
    }
    
    public void Initialize()
    {
        _view
            .OnNewGame(NewGame)
            .OnContinue(Continue)
            .SetContinue(CheckSave())
            .OnExit(Exit);
    }
    
    public MainMenuPresenter Update()
    {
        _view.SetContinue(CheckSave());
        
        return this;
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
        _signalBus.Fire(new SetSpaceshipDataSignal(_loadedData));
        _onContinue?.Invoke();
        Close();
    }
    
    private bool CheckSave() => _spaceshipDataManager.TryLoad(out _loadedData);
    
    private void Exit() => _signalBus.Fire(new QuitGameSignal());
}
}