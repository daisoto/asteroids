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
    private readonly int _maxValue;
    
    private Action _onContinue;
    private Action _onBack;
    
    public SpaceshipData SelectedData { get; private set; }
    
    public ShipSelectionPresenter(SpaceshipsData data, 
        ShipSelectionView view): base(view)
    {
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
        SelectedData = _data[index];
        _view.RepaintShip(SelectedData.Texture)
            .SetCharacteristics(
                SelectedData.MaxHealth, 
                SelectedData.Damage, 
                SelectedData.FireRate, 
                SelectedData.Speed);
    }
}
}