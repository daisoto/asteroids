using UniRx;
using UnityEngine;

namespace Gameplay
{
public interface ISpeedProvider
{
    public IReadOnlyReactiveProperty<Vector3> Speed { get; }
    
    void UpdateSpeed(Vector3 direction);
}
}