using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;

public class ShipSelectionPresenter: IInitializable
{
    private readonly IList<SpaceshipData> _data; 
    private readonly ShipSelectionView _view;
    private readonly int _maxValue;
    
    private Action _onContinue;
    
    public SpaceshipData SelectedData { get; private set; }
    
    public ShipSelectionPresenter(SpaceshipsData data, ShipSelectionView view)
    {
        _data = data.Data;
        _maxValue = data.MaxValue;
        _view = view;
    }
    
    public void Initialize()
    {
        _view.Draw(_data
            .Select(d => d.Title)
            .ToList(), SetSelected, _maxValue);
    }
    
    public ShipSelectionPresenter SetOnContinue(Action onContinue)
    {
        _onContinue = onContinue;
        
        return this;
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
