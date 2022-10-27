using System;
using System.Collections.Generic;

public abstract class Pool<T>: IPool<T>
{
    private readonly IFactory<T> _factory;
    private readonly Stack<T> _stack;

    protected Pool(IFactory<T> factory)
    {
        _factory = factory;
        _stack = new Stack<T>();
    }

    public T Get()
    {
        return _stack.Count < 1 ? 
            _factory.Get() : 
            _stack.Pop();
    }

    public void Return(T obj) => _stack.Push(obj);

    public void Clear() => _stack.Clear();
}

public abstract class Pool<T, V>: IPool<T, V> where V: Enum
{
    private readonly IFactory<T, V> _factory;
    private readonly Dictionary<V, Stack<T>> _stacks;

    protected Pool(IFactory<T, V> factory)
    {
        _factory = factory;
        
        _stacks = new Dictionary<V, Stack<T>>();
        var parameters = EnumUtils.GetValues<V>();
        foreach (var par in parameters)
        {
            var stack = new Stack<T>();
            _stacks.Add(par, stack);
        }
    }

    public T Get(V arg)
    {
        var stack = _stacks[arg];
        
        return stack.Count < 1 ? 
            _factory.Get(arg) : 
            stack.Pop();
    }

    public void Return(T obj, V arg) => _stacks[arg].Push(obj);

    public void Clear()
    {
        foreach (var stack in _stacks.Values)
            stack.Clear();
    }
}