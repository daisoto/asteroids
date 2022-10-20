using System;
using System.Collections.Generic;

public class AsteroidsPool: IPool<AsteroidModel>
{ 
    private readonly Func<AsteroidModel> _asteroidsProvider;
    private readonly Stack<AsteroidModel> _stack;
    
    public AsteroidsPool(Func<AsteroidModel> asteroidsProvider)
    {
        _asteroidsProvider = asteroidsProvider;
        _stack = new Stack<AsteroidModel>();
    }

    public AsteroidModel Get()
    {
        return _stack.Count < 1 ? 
            _asteroidsProvider.Invoke() : 
            _stack.Pop();
    }

    public void Return(AsteroidModel asteroid) => _stack.Push(asteroid);

    public void Clear()
    {
        _stack.Clear();
    }
}