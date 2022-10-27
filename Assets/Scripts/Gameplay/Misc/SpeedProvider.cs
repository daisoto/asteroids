using UniRx;
using UnityEngine;

namespace Gameplay
{
public abstract class SpeedProvider: IResettable
{
    public abstract IReadOnlyReactiveProperty<Vector3> Speed { get; }
    
    public abstract void UpdateSpeed(Vector3 direction);
    public void Reset() => UpdateSpeed(Vector3.zero);
}
}