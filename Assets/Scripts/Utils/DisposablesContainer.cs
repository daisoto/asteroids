using System.Collections.Generic;
using System;

public class DisposablesContainer: IDisposable
{
    public int Size => disposables.Count;

    private List<IDisposable> disposables = new List<IDisposable>();
    
    public void Dispose()
    {
        Clear();
    }

    public void Add(IDisposable disposable)
    {
        disposables.Add(disposable);
    }

    private void Clear()
    {
        disposables.ForEach(disposable => disposable?.Dispose());
        disposables.Clear();
    }
}
