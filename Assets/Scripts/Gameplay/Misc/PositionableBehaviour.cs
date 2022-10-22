using UnityEngine;

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
    
    public void SetSpeed(Vector3 speed) => _rigidbody.velocity = speed;
    
    public void AddForce(Vector3 force) =>  _rigidbody.AddForce(force);
}