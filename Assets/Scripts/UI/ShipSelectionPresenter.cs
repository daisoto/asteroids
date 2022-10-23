using System;
using System.Linq;
using System.Collections.Generic;

public class ShipSelectionPresenter: Presenter<ShipSelectionView>
{
    private readonly IList<SpaceshipData> _data; 
    private readonly int _maxValue;
    
    public SpaceshipData SelectedData { get; private set; }
    
    public ShipSelectionPresenter(SpaceshipsData data, 
        ShipSelectionView view): base(view)
    {
        _data = data.Data;
        _maxValue = data.MaxValue;
    }
    
    public void Initialize(Action onContinue, Action onBack)
    {
        _view
            .SetOnContinue(onContinue)
            .SetOnBack(onBack)
            .Draw(_data
                .Select(d => d.Title)
                .ToList(), SetSelected, _maxValue);
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
