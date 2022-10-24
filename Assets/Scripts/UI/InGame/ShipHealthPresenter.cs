using System;
using UniRx;
using Zenject;
using Gameplay;

namespace UI
{
public class ShipHealthPresenter: Presenter<CompoundBarView>, IInitializable, IDisposable
{
    private readonly SpaceshipModel _model;
    
    private IDisposable _disposable;

    public ShipHealthPresenter(SpaceshipModel model, 
        [Inject(Id = UIManager.SHIP_HEALTH_ID)]
        CompoundBarView view) 
        : base(view )
    {
        _model = model;
    }
    
    public void Initialize()
    {
        _view.Init(_model.MaxHealth);
        _disposable = _model.Health.Subscribe(_view.SetCells);
    }
    
    public void Dispose()
    {
        _disposable.Dispose();
    }
}
}