using UniRx;
using Zenject;

namespace Gameplay
{
public interface ISpeedProvider: IInitializable
{
    public IReadOnlyReactiveProperty<float> Speed { get; }
}
}