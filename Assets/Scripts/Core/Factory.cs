using System;

public class Factory<T>: IFactory<T>
{
    private readonly Func<T> _provider;
    
    public Factory(Func<T> provider)
    {
        _provider = provider;
    }

    public T Get() => _provider.Invoke();

    public IFactory<T> SetOnCreated(Action<T> onCreated)
    {
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