using UnityEngine;

namespace Gameplay
{
public class PositionableBehaviour: MonoBehaviour
{
    [SerializeField]
    protected Rigidbody _rigidbody;
    
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    public void SetActive(bool flag) => gameObject.SetActive(flag);
    
    public virtual void SetSpeed(Vector3 speed) => _rigidbody.velocity = speed;
}
}