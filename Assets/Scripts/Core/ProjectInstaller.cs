using System;
using System.Linq;
using UnityEngine;
using Zenject;

public interface ISignal { }

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    private SpaceshipsData _spaceshipsData;
    
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

        SignalBusInstaller.Install(Container);
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
