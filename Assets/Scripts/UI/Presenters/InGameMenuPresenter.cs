using System;
using Gameplay;

namespace UI
{ 
public class InGameMenuPresenter: Presenter<InGameMenuView>
{
    private readonly LevelController _levelController;
    
    public InGameMenuPresenter(InGameMenuView view, 
        LevelController levelController) : base(view)
    {
        _levelController = levelController;
    }

    public void Initialize(Action onExit)
    {
        _view
            .OnContinue(_levelController.ResumeGame)
            .OnExit(onExit) 
            .OnShow(_levelController.PauseGame);
    }
}
}