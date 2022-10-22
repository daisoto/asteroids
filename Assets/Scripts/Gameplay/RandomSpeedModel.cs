using UniRx;

public class RandomSpeedModel: ISpeedProvider
{
    private readonly ReactiveProperty<float> _speed;
    public IReadOnlyReactiveProperty<float> Speed => _speed;

    private readonly float _maxSpeed;
    private readonly float _minSpeed;
    
    public RandomSpeedModel(float maxSpeed, float minSpeed)
    {
        _maxSpeed = maxSpeed;
        _minSpeed = minSpeed;
        
        _speed = new ReactiveProperty<float>();
    }

    public void Initialize() => _speed.Value = RandomUtility.GetFloat(_minSpeed, _maxSpeed);
}