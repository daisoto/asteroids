using System;
using UniRx;
using Zenject;
using Gameplay;

namespace UI
{
public class ShipHealthPresenter: Presenter<CompoundBarView>, IInitializable, IDisposable
{
    private readonly SpaceshipController _controller;
    private readonly DisposablesContainer _disposablesContainer;

    public ShipHealthPresenter(SpaceshipController controller, 
        [Inject(Id = UIManager.SHIP_HEALTH_ID)]
        CompoundBarView view) 
        : base(view )
    {
        _controller = controller;
        _disposablesContainer = new DisposablesContainer();
    }
    
    public void Initialize()
    {
        _disposablesContainer.Add(_controller.MaxHealth
            .Subscribe(_view.Init));
        
        _disposablesContainer.Add(_controller.Health
            .Subscribe(_view.SetCells));
    }
    
    public void Dispose() => _disposablesContainer.Dispose();
}
}