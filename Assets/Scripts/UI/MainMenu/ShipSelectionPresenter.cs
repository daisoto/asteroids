using System;
using System.Linq;
using System.Collections.Generic;
using Data;
using Zenject;

namespace UI
{
public class ShipSelectionPresenter: Presenter<ShipSelectionView>, IInitializable
{
    private readonly IList<SpaceshipData> _data; 
    private readonly SignalBus _signalBus;
    private readonly int _maxValue;
    
    private Action _onContinue;
    private Action _onBack;
    
    private SpaceshipData _selectedData;
    
    public ShipSelectionPresenter(SpaceshipsData data, 
        ShipSelectionView view, SignalBus signalBus): base(view)
    {
        _signalBus = signalBus;
        _data = data.Data;
        _maxValue = data.MaxValue;
    }
    
    public void Initialize()
    {
        _view
            .SetOnContinue(Continue)
            .SetOnBack(Back)
            .Draw(_data
                .Select(d => d.Title)
                .ToList(), SetSelected, _maxValue);
    }
    
    public ShipSelectionPresenter SetOnContinue(Action onContinue)
    {
        _onContinue = onContinue;
        _signalBus.Fire(new SetSpaceshipDataSignal(_selectedData));
        
        return this;
    }
    
    public ShipSelectionPresenter SetOnBack(Action onBack)
    {
        _onBack = onBack;
        
        return this;
    }
    
    private void Continue()
    {
        _onContinue?.Invoke();
        Close();
    }
    
    private void Back()
    {
        _onBack?.Invoke();
        Close();
    }
    
    private void SetSelected(int index)
    {
        _selectedData = _data[index];
        _view.RepaintShip(_selectedData.Texture)
            .SetCharacteristics(
                _selectedData.MaxHealth, 
                _selectedData.Damage, 
                _selectedData.FireRate, 
                _selectedData.Speed);
    }
}
}