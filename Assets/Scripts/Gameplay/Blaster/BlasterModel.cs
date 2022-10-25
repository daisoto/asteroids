using System;
using UnityEngine;
using UniRx;

namespace Gameplay
{
public class BlasterModel: IDisposable
{
    public int Damage { get; }
    
    public float ProjectileSpeed { get; }
    
    private readonly float _firePeriod;
    
    private readonly IDisposable _updateObservation;
    
    private float _timer;
    
    public BlasterModel(int damage, int fireRate, float projectileSpeed)
    {
        Damage = damage;
        ProjectileSpeed = projectileSpeed;
        _firePeriod = 1f / fireRate;
        
        _updateObservation = Observable.EveryUpdate().Subscribe(_ =>
        {
            _timer += Time.deltaTime;
        });
    }
    
    public void Dispose() => _updateObservation.Dispose();
    
    public bool CanFire()
    {
        var canShoot = _timer >= _firePeriod;
        if (canShoot)
            _timer = 0;
        
        return canShoot;
    }
}
}