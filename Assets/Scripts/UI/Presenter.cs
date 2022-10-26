using System;

namespace UI
{
public abstract class Presenter<T> where T: View
{
    protected readonly T _view;
    
    public Presenter(T view)
    {
        _view = view;
    }

    public virtual void Show() => _view.Show();
    public virtual void Close() => _view.Close();
    
    public Presenter<T> SetOnShow(Action onShow)
    {
        _view.SetOnShow(onShow);
        
        return this;
    }
    
    public Presenter<T> SetOnClose(Action onClose)
    {
        _view.SetOnClose(onClose);
        
        return this;
    }
}
}