using System;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Data
{
[CreateAssetMenu(fileName = "New AsteroidsData", menuName = "Asteroids data")]
public class AsteroidsSettings: ScriptableObject, 
    IFactory<AsteroidBehaviour, AsteroidSize>
{
    [SerializeField]
    private AsteroidData[] _asteroidsData;
    
    public IList<AsteroidData> Data => _asteroidsData;
    
    [SerializeField]
    private int _minAsteroidsNum;
    public int MinAsteroidsNum => _minAsteroidsNum;
    
    [SerializeField]
    private int _maxAsteroidsNum;
    public int MaxAsteroidsNum => _maxAsteroidsNum;
    
    public AsteroidBehaviour Get(AsteroidSize size)
    {
        AsteroidBehaviour behaviour = null;
        
        foreach (var data in _asteroidsData)
            if (data.Size == size)
                behaviour = Instantiate(data.Prefab);
        
        return behaviour;
    }

    public IFactory<AsteroidBehaviour, AsteroidSize> 
        SetOnCreated(Action<AsteroidBehaviour, AsteroidSize> onCreated) 
    { return this; }
}

[Serializable]
public struct AsteroidData
{
    [SerializeField]
    private AsteroidSize _size;
    public AsteroidSize Size => _size;
    
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth => _maxHealth;
    
    [SerializeField]
    private float _minSpeed;
    public float MinSpeed => _minSpeed;
    
    [SerializeField]
    private float _maxSpeed;
    public float MaxSpeed => _maxSpeed;
    
    [Header("Should equals 1 due to conditions")]
    [SerializeField]
    private int _damage;
    public int Damage => _damage;
    
    [SerializeField]
    private AsteroidBehaviour _prefab; 
    public AsteroidBehaviour Prefab => _prefab;
}
}