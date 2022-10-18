using System.Collections.Generic;
using System;

public class DisposablesContainer
{
    public int size => disposables.Count;

    private List<IDisposable> disposables = new List<IDisposable>();

    ~DisposablesContainer()
    {
        Clear();
    }

    public void Add(IDisposable disposable)
    {
        disposables.Add(disposable);
    }

    public void Clear()
    {
        disposables.ForEach(disposable => disposable?.Dispose());
        disposables.Clear();
    }
}
