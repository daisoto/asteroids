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
    private SpaceshipsSettings _spaceshipsSettings;
    
    [SerializeField]
    private AsteroidsSettings _asteroidsSettings;
    
    [SerializeField]
    private LevelsSettings _levelsSettings;

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
        Container.BindInterfacesAndSelfTo<SpaceshipsSettings>()
            .FromInstance(_spaceshipsSettings);
        
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
        
        Container.BindInterfacesAndSelfTo<EndGamePresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<UIManager>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindLevels()
    {
        Container.BindInterfacesAndSelfTo<LevelsSettings>()
            .FromInstance(_levelsSettings);
        
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
        Container.BindInterfacesAndSelfTo<AsteroidsSettings>()
            .FromInstance(_asteroidsSettings);
        
        Container.BindInterfacesTo<AsteroidsFactory>()
            .AsSingle();
        
        Container.Bind<AsteroidsNumProvider>()
            .WithId(AsteroidSize.Small)
            .FromInstance(new SmallAsteroidsNumProvider(_levelsSettings.MaxLevel))
            .NonLazy();

        Container.Bind<AsteroidsNumProvider>()
            .WithId(AsteroidSize.Medium)
            .FromInstance(new MediumAsteroidsNumProvider(_levelsSettings.MaxLevel))
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<AsteroidsController>()
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