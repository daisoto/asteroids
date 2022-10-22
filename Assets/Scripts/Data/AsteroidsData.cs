using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AsteroidsData", menuName = "Asteroids data")]
public class AsteroidsData: ScriptableObject, 
    IFactory<AsteroidBehaviour, AsteroidSize>,
    ITotalAsteroidsProvider
{
    [SerializeField]
    private AsteroidData[] _asteroidsData;
    
    public IList<AsteroidData> Data => _asteroidsData;
    
    [SerializeField]
    private int _maxLevel;
    public int MaxLevel => _maxLevel;
    
    [SerializeField]
    private int _minAsteroidsNum;
    
    [SerializeField]
    private int _maxAsteroidsNum;
    
    private int _minLevel => 1;

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

    public int Get(int level) 
    {
        var lerp = Mathf.RoundToInt(
            Mathf.Lerp(_minAsteroidsNum, _maxAsteroidsNum, 
                (float)level / _maxLevel));
        var delta = (_maxAsteroidsNum - _minAsteroidsNum) / 2;
        var left = lerp - delta; 
        var right = lerp + delta;
        var random = RandomUtils.GetInt(left, right);
        
        return Mathf.Clamp(random, _minAsteroidsNum, _maxAsteroidsNum);
    }
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