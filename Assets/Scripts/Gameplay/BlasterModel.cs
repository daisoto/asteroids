using UnityEngine;
using Zenject;

public class BlasterModel: ITickable
{
    private readonly int _damage;
    private readonly float _firePeriod;
    
    private float _timer;
    
    public BlasterModel(int damage, int fireRate)
    {
        _damage = damage;
        _firePeriod = 1f / fireRate;
    }
    
    public bool TryToFire(out int damage)
    {
        damage = _damage;
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