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
}
}