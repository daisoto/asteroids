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
    
    [SerializeField]
    private ProjectileBehaviour _projectileBehaviourPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(_spaceshipsData);
        Container.BindInstance(_projectileBehaviourPrefab);
        Container.BindInstance(_levelsData);
        
        BindSignals();
        BindLevels();
        BindAsteroids();
        BindUI();
        
        Container.BindInterfacesAndSelfTo<InputManager>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<SpaceshipDataManager>()
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
        
        Container.BindInterfacesAndSelfTo<LevelSelectionPresenter>()
            .AsSingle()
            .NonLazy();
        
        Container.BindInterfacesAndSelfTo<UIManager>()
            .AsSingle()
            .NonLazy();
    }
    
    private void BindLevels()
    {
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