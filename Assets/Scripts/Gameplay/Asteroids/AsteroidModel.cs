﻿using System;
using UniRx;
using UnityEngine;

namespace Gameplay
{
public class AsteroidModel: SpaceModel, IResettable
{
    private readonly HealthModel _healthModel;
    private readonly SpeedProvider _speedProvider;
    
    public IObservable<Unit> Destroy => _destroy;
    private readonly ReactiveCommand _destroy;
    public IObservable<Unit> Explode => _explode;
    private readonly ReactiveCommand _explode;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    
    public AsteroidSize Size { get;}
    public int Damage { get; }
    public float RotationTumble { get; }
    public Vector3 ExplosionStrength { get; }

    public AsteroidModel(HealthModel healthModel, SpeedProvider speedProvider, 
        AsteroidSize size, int damage, float tumble, Vector3 explosionStr)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
        Size = size;
        Damage = damage;
        RotationTumble = tumble;
        ExplosionStrength = explosionStr;
        
        _destroy = new ReactiveCommand();
        _explode = new ReactiveCommand();
        
        _healthModel.SetOnDeath(Collide);
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
    
    public void DecreaseHealth(int damage)
    {
        _healthModel.DecreaseHealth(damage);
        _explode.Execute();
    }
    
    public void Collide() => _destroy.Execute();
    public void Reset()
    {
        _speedProvider.Reset();
        _healthModel.Reset();
        Activate();
    }
}
}