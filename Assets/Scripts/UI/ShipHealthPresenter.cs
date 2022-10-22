using System;
using UniRx;
using Zenject;

public class ShipHealthPresenter: IInitializable, IDisposable
{
    private readonly SpaceshipModel _model;
    private readonly CompoundBarView _view;
    
    private IDisposable _disposable;

    public ShipHealthPresenter(SpaceshipModel model, CompoundBarView view)
    {
        _model = model;
        _view = view;
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