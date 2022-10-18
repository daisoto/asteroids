using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}