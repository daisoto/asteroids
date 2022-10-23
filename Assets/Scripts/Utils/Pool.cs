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