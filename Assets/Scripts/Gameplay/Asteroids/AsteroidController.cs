using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Gameplay
{
public class AsteroidController: IDisposable
{
    private readonly AsteroidModel _model;
    private readonly AsteroidBehaviour _behaviour;
    
    private readonly DisposablesContainer _disposablesContainer;
    
    private Action<AsteroidSize, Vector3> _onExplode;
    private Action<AsteroidModel> _onDeactivate;

    public AsteroidController(AsteroidModel model, 
        AsteroidBehaviour behaviour)
    {
        _model = model;
        _behaviour = behaviour;

        _disposablesContainer = new DisposablesContainer();
        
        BindBehaviour();
    }
    
    public void Dispose() => _disposablesContainer.Dispose();
    
    public AsteroidController SetOnExplode(
        Action<AsteroidSize, Vector3> onExplode)
    {
        _onExplode = onExplode;
        
        return this;
    }
    
    public AsteroidController SetOnDeactivate(
        Action<AsteroidModel> onDeactivate)
    {
        _onDeactivate = onDeactivate;
        
        return this;
    }
    
    private void BindBehaviour()
    {
        _behaviour
            .SetDamage(_model.Damage)
            .SetOnDamage(_model.DecreaseHealth)
            .SetOnCollide(_model.Collide)
            .SetActive(false);
        
        _disposablesContainer.Add(_model.IsActive
            .SkipLatestValueOnSubscribe()
            .Subscribe(isActive =>
            {
                if (isActive)
                {
                    _behaviour.SetBaseModel(true);
                    _behaviour.SetActive(true);
                    var horizontal = 
                        RandomUtils.ProcessProbability(0.5) ?
                            Vector3.right : Vector3.left;
                    var vertical = 
                        RandomUtils.ProcessProbability(0.5) ? 
                            Vector3.forward : Vector3.back;
                    _model.UpdateSpeed(vertical + horizontal);
                }
                else
                {
                    _onDeactivate?.Invoke(_model);
                    _behaviour.SetActive(false);
                }
            }));
        
        _disposablesContainer.Add(_model.Speed
            .Subscribe(speed => _behaviour.SetSpeed(speed)));
        
        _disposablesContainer.Add(_model.Position
            .Subscribe(position => _behaviour.Position = position)); 
        
        _disposablesContainer.Add(_model.Destroy
            .Subscribe(i => Destroy().Forget()));
        
        _disposablesContainer.Add(_model.Explode
            .Subscribe(i => _behaviour.ToggleExplosion()));
    }
    
    private async UniTask Destroy()
    {
        _behaviour.SetBaseModel(false);
        _onExplode?.Invoke(_model.Size, _behaviour.Position);
        await _behaviour.ToggleExplosionAsync();
        if (_model.IsActive.Value)
            _model.Deactivate();
    }
}
}