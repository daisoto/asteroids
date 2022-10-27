using System;

public interface IPool<T>
{
    T Get();
    void Return(T obj);
    void Clear();
}

public interface IPool<T, in V> where V: Enum
{
    T Get(V arg);
    void Return(T obj, V arg);
    void Clear();
}