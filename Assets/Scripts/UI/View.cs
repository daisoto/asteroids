using UnityEngine;

public class View: MonoBehaviour
{
    [SerializeField]
    protected GameObject _root;
    
    public virtual void Show() => _root.SetActive(true);
    
    public virtual void Close() => _root.SetActive(false); 
}