using UniRx;

namespace Gameplay
{
public class PlanetModel
{
    public ReactiveProperty<bool> IsAvailable { get; }
    public ReactiveProperty<bool> IsFinished { get; }
    
    public PlanetModel(bool isAvailable, bool isFinished)
    {
        IsAvailable = new ReactiveProperty<bool>(isAvailable);
        IsFinished = new ReactiveProperty<bool>(isFinished);
    }
}
}