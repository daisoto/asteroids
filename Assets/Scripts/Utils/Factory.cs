using System;

public class Factory<T>: IFactory<T>
{
    private readonly Func<T> _provider;
    
    public Factory(Func<T> provider)
    {
        _provider = provider;
    }

    public T Get() 
    {
        var obj = _provider.Invoke();
        
        return obj;
    }
}