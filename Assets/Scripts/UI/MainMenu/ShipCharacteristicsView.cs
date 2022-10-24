using UnityEngine;

namespace UI
{
public class ShipCharacteristicsView: View
{
    [SerializeField]
    private CompoundBarView _healthBarView;
    [SerializeField]
    private CompoundBarView _damageBarView;
    [SerializeField]
    private CompoundBarView _fireRateBarView;
    [SerializeField]
    private CompoundBarView _speedBarView;
    
    public void Init(int maxNum)
    {
        _healthBarView.Init(maxNum);
        _damageBarView.Init(maxNum);
        _fireRateBarView.Init(maxNum);
        _speedBarView.Init(maxNum);
    }
    
    public void SetHealth(int num) => _healthBarView.SetCells(num);
    public void SetDamage(int num) => _damageBarView.SetCells(num);
    public void SetFireRate(int num) => _fireRateBarView.SetCells(num);
    public void SetSpeed(int num) => _speedBarView.SetCells(num);
}
}