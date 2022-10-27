using UnityEngine;

namespace Gameplay
{
public class SpaceBehaviour: MonoBehaviour
{
    [SerializeField]
    private GameObject _baseModel;
    
    [SerializeField]
    private Collider _collider;
    
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
        set => transform.rotation = value.normalized;
    }
    
    public Vector3 Forward => transform.forward;
    
    public void SetActive(bool flag) 
    {
        if (gameObject)
            gameObject.SetActive(flag);
    }
    
    public void SetBaseModel(bool flag) 
    {
        _collider.enabled = flag;
        _baseModel.SetActive(flag);
    }

    public void SetSpeed(Vector3 speed) => _rigidbody.velocity = speed;
}
}