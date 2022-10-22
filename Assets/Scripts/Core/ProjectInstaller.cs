using System;
using System.Linq;
using UnityEngine;
using Zenject;

public interface ISignal { }

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    private SpaceshipsData _spaceshipsData;
    
    [SerializeField]
    private AsteroidsData _asteroidsData;
    
    [SerializeField]
    private ProjectileBehaviour _projectileBehaviourPrefab;

    public override void InstallBindings()
    {
        BindSignals();
        
        Container.BindInstance(_spaceshipsData);
        
        Container.BindInterfacesAndSelfTo<InputManager>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<BlasterModel>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<AccelerationModel>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<HealthModel>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerController>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<SpaceshipController>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<AsteroidsController>()
            .AsSingle()
            .NonLazy();
        
        BindAsteroids();

        SignalBusInstaller.Install(Container);
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
    }
}
