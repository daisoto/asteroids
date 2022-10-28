using System;
using Zenject;
using Gameplay;

namespace UI
{ 
public class EndGamePresenter: Presenter<EndGameView>, 
    IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    
    private Action _toLevels;

    public EndGamePresenter(EndGameView view, SignalBus signalBus) : base(view)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SpaceshipDestroyedSignal>(OnSpaceShipDestroyed);
        _signalBus.Subscribe<LevelPassedSignal>(OnLevelPassed);
        
        _view.SetToLevels(ToLevels);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<SpaceshipDestroyedSignal>(OnSpaceShipDestroyed);
        _signalBus.Unsubscribe<LevelPassedSignal>(OnLevelPassed);
    }
    
    public EndGamePresenter SetToLevels(Action toLevels)
    {
        _toLevels = toLevels;
        
        return this;
    }
    
    private void OnSpaceShipDestroyed(SpaceshipDestroyedSignal signal) =>
        _view.ShowFail();
    
    private void OnLevelPassed(LevelPassedSignal signal) =>
        _view.ShowSuccess();
    
    private void ToLevels()
    {
        Close();
        _toLevels?.Invoke();
        _signalBus.Fire(new LevelEndedSignal());
    }
}
}