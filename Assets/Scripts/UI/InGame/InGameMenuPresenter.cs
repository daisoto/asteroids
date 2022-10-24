﻿using System;
using Gameplay;
using Zenject;

namespace UI
{ 
public class InGameMenuPresenter: Presenter<InGameMenuView>, IInitializable
{
    private readonly SignalBus _signalBus;
    
    private Action _onExit;
    
    public InGameMenuPresenter(InGameMenuView view, 
        SignalBus signalBus) : base(view)
    {
        _signalBus = signalBus;
    }
    
    public InGameMenuPresenter SetOnExit(Action onExit)
    {
        _onExit = onExit;
        
        return this;
    }

    public void Initialize()
    {
        
        _view
            .OnContinue(Resume)
            .OnExit(Exit) 
            .SetOnShow(Pause);
    }
    
    private void Exit()
    {
        Close();
        _onExit?.Invoke();
    }
    
    private void Resume()
    {
        _signalBus.Fire(new ResumeGameSignal());
    }
    
    private void Pause()
    {
        _signalBus.Fire(new PauseGameSignal());
    }
}
}