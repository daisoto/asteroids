using System;

public class Factory<T>: IFactory<T>
{
    private readonly Func<T> _provider;
    
    private Action<T> _onCreated;
    
    public Factory(Func<T> provider)
    {
        _provider = provider;
    }

    public T Get() 
    {
        var obj = _provider.Invoke();
        _onCreated?.Invoke(obj);
        
        return obj;
    }

    public IFactory<T> SetOnCreated(Action<T> onCreated)
    {
        _onCreated = onCreated;
        
        return this;
    }
}

public abstract class Factory<T, V>: IFactory<T, V> where V : Enum
{
    private Action<T, V> _onCreated;
    
    public T Get(V arg)
    {
        T obj = Create(arg);
        _onCreated?.Invoke(obj, arg);
        
        return obj;
    }

    public IFactory<T, V> SetOnCreated(Action<T, V> onCreated)
    {
        _onCreated = onCreated;
        
        return this;
    }
    
    protected abstract T Create(V arg);
}