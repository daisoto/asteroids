using UnityEngine;
using Zenject;
using UI;
using Gameplay;
using Data;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField]
    private Camera _camera;
    
    [Inject]
    private ShipSelectionPresenter _shipSelectionPresenter;
    
    [Inject]
    private SpaceshipDataManager _spaceshipDataManager;

    public override void InstallBindings()
    {
        Container.BindInstance(_camera);
        
        BindSpaceship();

        Container.BindInterfacesAndSelfTo<PlayerController>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindSpaceship()
    {
        Container.BindInstance(GetSpaceshipModel());
        
        Container.BindInterfacesAndSelfTo<SpaceshipController>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInstance(GetBlasterModel());

        Container.BindInterfacesAndSelfTo<BlasterController>()
            .AsSingle()
            .NonLazy();
    }
    
    private SpaceshipModel GetSpaceshipModel()
    {
        var data  = GetCurrentSpaceshipData();
        
        var healthModel = new HealthModel(data.MaxHealth);
        var speedProvider = new UniformSpeedProvider(data.Speed);
        
        return new SpaceshipModel(
            healthModel, speedProvider, data.Texture);
    }
    
    private BlasterModel GetBlasterModel()
    {
        var data  = GetCurrentSpaceshipData();
        
        return new BlasterModel(data.Damage, data.FireRate, data.ProjectileSpeed);
    }
    
    private SpaceshipData GetCurrentSpaceshipData()
    {
        return _spaceshipDataManager.TryLoad(out var data) ? 
            data : _shipSelectionPresenter.SelectedData;
    }
}