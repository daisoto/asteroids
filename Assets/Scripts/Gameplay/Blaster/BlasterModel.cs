using UnityEngine;
using Zenject;

public class BlasterModel: ITickable
{
    public int Damage { get; }
    
    private readonly float _firePeriod;
    
    private float _timer;
    
    public BlasterModel(int damage, int fireRate)
    {
        Damage = damage;
        _firePeriod = 1f / fireRate;
    }
    
    public bool CanFire()
    {
        var canShoot = _timer >= _firePeriod;
        if (canShoot)
            _timer = 0;
        
        return canShoot;
    }
    
    public void Tick()
    {
        _timer += Time.deltaTime;
    }
}