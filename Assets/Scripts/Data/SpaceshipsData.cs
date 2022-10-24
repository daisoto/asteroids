using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Gameplay;

namespace Data
{
[CreateAssetMenu(fileName = "New SpaceshipsData", menuName = "Spaceships data")]
public class SpaceshipsData : ScriptableObject, ITextureProvider
{
    [SerializeField]
    private SpaceshipData[] _spaceShipsData;
    public IList<SpaceshipData> Data => _spaceShipsData;
    
    [SerializeField]
    private TextureData[] _texturesData;
    
    [SerializeField]
    private ProjectileBehaviour _projectileBehaviourPrefab;
    
    public int MaxValue => 3;

    public ProjectileBehaviour GetProjectileBehaviour() => 
        Instantiate(_projectileBehaviourPrefab);

    public Texture2D Get(string id)
    {
        return (from td in _texturesData 
            where td.Id == id 
            select td.Texture)
            .FirstOrDefault();
    }
}

[Serializable]
public class SpaceshipData
{
    [SerializeField]
    private string _title;
    public string Title { get => _title; set => _title = value; }

    [SerializeField, Range(1, 3)]
    private int _damage;
    public int Damage 
    { get => _damage; set => _damage = value; }
    
    [SerializeField, Range(1, 3)]
    private int _fireRate;
    public int FireRate 
    { get => _fireRate; set => _fireRate = value; }
    
    [SerializeField, Range(1, 3)]
    private int _speed;
    public int Speed 
    { get => _speed; set => _speed = value; }
    
    [Header("Should equals 3 due to conditions")]
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth 
    { get => _maxHealth; set => _maxHealth = value; }
    
    [SerializeField]
    private int _projectileSpeed;
    public int ProjectileSpeed 
    { get => _projectileSpeed; set => _projectileSpeed = value; }
}

[Serializable]
public class TextureData
{
    [SerializeField]
    private string _id;
    public string Id => _id;
    
    [SerializeField]
    private Texture2D _texture;
    public Texture2D Texture => _texture;
}
}