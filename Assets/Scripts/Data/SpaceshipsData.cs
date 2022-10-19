using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpaceshipsData", menuName = "Spaceships data")]
public class SpaceshipsData : ScriptableObject
{
    [SerializeField]
    private SpaceshipData[] _spaceShipData;
    
    public IList<SpaceshipData> SpaceShipData => _spaceShipData;
}

[Serializable]
public struct SpaceshipData
{
    [SerializeField]
    private string _title;
    public string Title => _title;
    
    [SerializeField]
    private Texture2D _texture;
    public Texture2D Texture => _texture;

    [SerializeField, Range(1, 3)]
    private int _damage;
    public int Damage => _damage;
    
    [SerializeField, Range(1, 3)]
    private int _fireRate;
    public int FireRate => _fireRate;
    
    [SerializeField, Range(1, 3)]
    private int _speed;
    public int Speed => _speed;
}
