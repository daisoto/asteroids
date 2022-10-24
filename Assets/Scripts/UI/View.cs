using System;
using UnityEngine;

namespace UI
{
public class View: MonoBehaviour
{
    [SerializeField]
    protected GameObject _root;
    
    protected Action _onShow;
    protected Action _onClose;
    
    public View SetOnShow(Action onShow)
    {
        _onShow = onShow;
        
        return this;
    }
    
    public View SetOnClose(Action onClose)
    {
        _onClose = onClose;
        
        return this;
    }
    
    public virtual void Show() 
    {
        _root.SetActive(true);
        _onShow?.Invoke();
    }
    
    public virtual void Close()
    {
        _root.SetActive(false); 
        _onClose?.Invoke();
    }
}
}