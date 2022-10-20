using System;

public interface IFactory<out T, V> where V: Enum
{
    T Get(V arg);
    
    void SetOnCreated(Action<T, V> onCreated);
}