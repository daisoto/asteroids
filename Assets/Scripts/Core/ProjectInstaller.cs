using System;
using System.Linq;
using UnityEngine;
using Zenject;
using UI;
using Gameplay;
using Data;

public interface ISignal { }

namespace Core
{
public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    private SpaceshipsData _spaceshipsData;
    
    [SerializeField]
    private AsteroidsData _asteroidsData;
    
    [SerializeField]
    private LevelsData _levelsData;

    public override void InstallBindings()
    {
        BindSignals();
        BindLevels();
        BindUI();
        BindSpaceship();
        BindAsteroids();
        
        Container.BindInterfacesAndSelfTo<InputManager>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerController>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<GameplayController>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindSpaceship()
    {
        Container.BindInstance(_spaceshipsData);
        
        Container.BindInterfacesAndSelfTo<SpaceshipDataManager>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<SpaceshipController>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<BlasterController>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindUI()
    {
        Container.BindInterfacesAndSelfTo<MainMenuPresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<ShipSelectionPresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<MapPresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<ShipHealthPresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<InGameMenuPresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<UIManager>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindLevels()
    {
        Container.BindInstance(_levelsData);
        
        Container.BindInterfacesAndSelfTo<LevelsDataManager>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<LevelsController>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<LevelController>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindAsteroids()
    {
        Container.BindInstance(_asteroidsData);
        
        Container.BindInterfacesTo<IFactory<AsteroidBehaviour, AsteroidSize>>()
            .FromInstance(_asteroidsData);
        
        Container.BindInterfacesTo<AsteroidsFactory>()
            .AsSingle();
        
        Container.Bind<AsteroidsNumProvider>()
            .WithId(AsteroidSize.Small)
            .To<SmallAsteroidsNumProvider>();

        Container.Bind<AsteroidsNumProvider>()
            .WithId(AsteroidSize.Medium)
            .To<MediumAsteroidsNumProvider>();
        
        Container.BindInterfacesAndSelfTo<AsteroidsController>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<TotalAsteroidsProvider>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindSignals()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ISignal).IsAssignableFrom(p)
                        && !p.IsInterface
                        && !p.IsAbstract);                        
                        
        foreach (var type in types) 
            Container.DeclareSignal(type);

        SignalBusInstaller.Install(Container);
    }
}
}