using System;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class GameplayController: IInitializable, IDisposable 
{
    private readonly SignalBus _signalBus;
    private readonly PlayerController _playerController;

    public GameplayController(SignalBus signalBus, 
        PlayerController playerController)
    {
        _signalBus = signalBus;
        _playerController = playerController;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<ResumeGameSignal>(ResumeGame);
        _signalBus.Subscribe<PauseGameSignal>(PauseGame);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<ResumeGameSignal>(ResumeGame);
        _signalBus.Unsubscribe<PauseGameSignal>(PauseGame);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        _playerController.SetActive(false);
    }
    
    private void ResumeGame()
    {
        Time.timeScale = 1;
        _playerController.SetActive(true);
    }
}
}