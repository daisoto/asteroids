using UniRx;
using Zenject;

public interface ISpeedProvider: IInitializable
{
    public IReadOnlyReactiveProperty<float> Speed { get; }
}