using System;

public interface IFactory<out T> 
{
    T Get();
    
    IFactory<T> SetOnCreated(Action<T> onCreated);
}

public interface IFactory<out T, V> where V: Enum
{
    T Get(V arg);
    
    IFactory<T, V> SetOnCreated(Action<T, V> onCreated);
}