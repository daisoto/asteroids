using UnityEngine;

public class AsteroidBehaviour: MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    
    private AsteroidModel _asteroidModel;
    
    public int Damage => _asteroidModel.Damage;
    
    public AsteroidBehaviour SetAsteroidModel(AsteroidModel asteroidModel)
    {
        _asteroidModel = asteroidModel;
        
        return this;
    }
    
    public AsteroidBehaviour SetSpeed(Vector2 speed)
    {
        _rigidbody.velocity = speed;
        
        return this;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ProjectileBehaviour projectile))
        {
            if (_asteroidModel.Size == AsteroidSize.Big)
            {
                // раскалывается на 2 маленьких
                // уничтожается
            }
            else
            {
                // уничтожается 
                // + очки? 
            }
        }
    }
}