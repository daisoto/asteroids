using UnityEngine;

namespace Gameplay
{
public class SpaceBehaviour: MonoBehaviour
{
    [SerializeField]
    protected Rigidbody _rigidbody;
    
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    public Quaternion Rotation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }
    
    public Vector3 Forward => transform.forward;
    
    public void SetActive(bool flag) => gameObject.SetActive(flag);
    
    public void SetSpeed(Vector3 speed) => _rigidbody.velocity = speed;
}
}