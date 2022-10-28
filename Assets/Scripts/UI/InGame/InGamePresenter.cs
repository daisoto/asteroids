using System;
using Gameplay;
using Zenject;
using UniRx;

namespace UI
{ 
public class InGamePresenter: Presenter<InGameView>, IInitializable, IDisposable
{
    private readonly CompoundBarView _healthBar;
    private readonly SpaceshipController _controller;
    private readonly SignalBus _signalBus;
    
    private readonly DisposablesContainer _disposablesContainer;
    
    public InGamePresenter(InGameView view,
        [Inject(Id = UIManager.SHIP_HEALTH_ID)]
        CompoundBarView healthBar, 
        SignalBus signalBus, 
        SpaceshipController controller) : base(view)
    {
        _healthBar = healthBar;
        _signalBus = signalBus;
        _controller = controller;
        
        _disposablesContainer = new DisposablesContainer();
    }

    public void Initialize()
    {
        _view 
            .SetOnShowMenu(Pause)
            .SetOnContinue(Resume)
            .SetOnExit(Exit);

        _disposablesContainer.Add(_controller.MaxHealth
            .Subscribe(_healthBar.Construct));
        _disposablesContainer.Add(_controller.Health
            .Subscribe(health =>
            {
                _healthBar.SetCells(health);
                if (health < 1)
                    Close();
            }));
        
        _signalBus.Subscribe<LevelStartedSignal>(Show);
        _signalBus.Subscribe<SpaceshipDestroyedSignal>(Close);
        _signalBus.Subscribe<LevelEndedSignal>(Close);
    }
    
    public void Dispose() 
    {
        _disposablesContainer.Dispose();
        _signalBus.Unsubscribe<LevelStartedSignal>(Show);
        _signalBus.Unsubscribe<SpaceshipDestroyedSignal>(Close);
        _signalBus.Unsubscribe<LevelEndedSignal>(Close);
    }
    
    public void ShowMenu() => _view.ShowMenu();

    private void Exit()
    {
        _view.CloseMenu();
        Close();
        _signalBus.Fire(new LevelEndedSignal());
    }
    
    private void Resume() => _signalBus.Fire(new ResumeGameSignal());
    
    private void Pause() => _signalBus.Fire(new PauseGameSignal());
}
}