using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    private SpaceshipsData _spaceshipsData;
    
    public override void InstallBindings()
    {
        Debug.Log("Binding Project Container");
        
        Container.BindInstance(_spaceshipsData);
        
        Container.BindInterfacesAndSelfTo<InputManager>()
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<AudioController>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();

        SignalBusInstaller.Install(Container);
    }
}
