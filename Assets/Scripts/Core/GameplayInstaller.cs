using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField]
    private Camera _camera;
    
    [Inject]
    private ShipSelectionPresenter _shipSelectionPresenter;

    public override void InstallBindings()
    {
        Container.BindInstance(_camera);
        
        Container.BindInstance(GetSpaceshipModel());
        
        Container.BindInterfacesAndSelfTo<SpaceshipController>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<BlasterController>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerController>()
            .AsSingle()
            .NonLazy();
    }
    
    private SpaceshipModel GetSpaceshipModel()
    {
        var data  = _shipSelectionPresenter.SelectedData;
        
        var healthModel = new HealthModel(data.MaxHealth);
        var speedProvider = new UniformSpeedProvider(data.Speed);
        
        return new SpaceshipModel(
            healthModel, speedProvider, data.Texture);
    }
}