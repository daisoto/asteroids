using UniRx;

namespace Gameplay
{
public interface ISpeedProvider
{
    public IReadOnlyReactiveProperty<float> Speed { get; }
    
    void UpdateSpeed();
}
}