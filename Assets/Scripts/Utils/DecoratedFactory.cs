using System;

public class DecoratedFactory<T>: IFactory<T>
{
    private readonly IFactory<T> _factoryInternal;
    private Action<T> _onCreated;

    public DecoratedFactory(IFactory<T> factoryInternal)
    {
        _factoryInternal = factoryInternal;
    }
    
    public IFactory<T> SetOnCreated(Action<T> onCreated)
    {
        _onCreated = onCreated;
        
        return this;
    }

    public T Get()
    {
        T obj = _factoryInternal.Get();
        _onCreated?.Invoke(obj);
        
        return obj;
    }
}

public class DecoratedFactory<T, V>: IFactory<T, V> where V : Enum
{
    private readonly IFactory<T, V> _factoryInternal;
    private Action<T, V> _onCreated;

    public DecoratedFactory(IFactory<T, V> factoryInternal)
    {
        _factoryInternal = factoryInternal;
    }

    public IFactory<T, V> SetOnCreated(Action<T, V> onCreated)
    {
        _onCreated = onCreated;
        
        return this;
    }

    public T Get(V arg)
    {
        T obj = _factoryInternal.Get(arg);
        _onCreated?.Invoke(obj, arg);
        
        return obj;
    }
}