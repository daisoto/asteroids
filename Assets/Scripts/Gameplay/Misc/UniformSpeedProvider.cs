using UniRx;

namespace Gameplay
{
public class UniformSpeedProvider: ISpeedProvider
{
    public IReadOnlyReactiveProperty<float> Speed => _speed;
    private readonly ReactiveProperty<float> _speed;
    
    private readonly float _speedInternal;
    
    public UniformSpeedProvider(float speed)
    {
        _speed = new ReactiveProperty<float>();
        _speedInternal = speed;
    }
    
    public void Initialize()
    {
        _speed.SetValueAndForceNotify(_speedInternal);
    }
}
}