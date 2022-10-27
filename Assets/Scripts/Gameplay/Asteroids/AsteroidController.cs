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
    private readonly AsteroidSize _size;
    
    private readonly DisposablesContainer _disposablesContainer;
    
    private Action<AsteroidSize, Vector3> _onDestroy;
    private Action<AsteroidSize, AsteroidModel> _onDeactivate;

    public AsteroidController(AsteroidModel model, 
        AsteroidBehaviour behaviour, AsteroidSize size)
    {
        _model = model;
        _behaviour = behaviour;
        _size = size;

        _disposablesContainer = new DisposablesContainer();
        
        BindBehaviour();
    }
    
    public void Dispose() => _disposablesContainer.Dispose();
    
    public AsteroidController SetOnDestroy(
        Action<AsteroidSize, Vector3> onDestroy)
    {
        _onDestroy = onDestroy;
        
        return this;
    }
    
    public AsteroidController SetOnDeactivate(
        Action<AsteroidSize, AsteroidModel> onDeactivate)
    {
        _onDeactivate = onDeactivate;
        
        return this;
    }
    
    private void BindBehaviour()
    {
        _behaviour
            .SetDamage(_model.Damage)
            .SetOnDamage(_model.DecreaseHealth)
            .SetOnCollide(_model.Deactivate)
            .SetActive(false);
        
        _disposablesContainer.Add(_model.IsActive
            .SkipLatestValueOnSubscribe()
            .Subscribe(isActive =>
            {
                if (!isActive)
                {
                    _onDeactivate?.Invoke(_size, _model);
                    _behaviour.SetActive(false);
                }
                else
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
        var position = _behaviour.Position;
        _behaviour.SetBaseModel(false);
        await _behaviour.ToggleExplosionAsync();
        if (_behaviour)
            _behaviour.SetActive(false);
        _onDestroy?.Invoke(_size, position);
    }
}
}