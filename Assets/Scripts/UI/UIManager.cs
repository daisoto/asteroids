﻿using Zenject;

namespace UI
{
public class UIManager: IInitializable
{
    public const string SHIP_HEALTH_ID = "ship_health";
    
    private readonly MainMenuPresenter _mainMenuPresenter;
    private readonly ShipSelectionPresenter _shipSelectionPresenter;
    private readonly MapPresenter _mapPresenter;
    private readonly InGameMenuPresenter _inGameMenuPresenter;
    
    public UIManager(MainMenuPresenter mainMenuPresenter, 
        ShipSelectionPresenter shipSelectionPresenter, 
        MapPresenter mapPresenter, 
        InGameMenuPresenter inGameMenuPresenter)
    {
        _mainMenuPresenter = mainMenuPresenter;
        _shipSelectionPresenter = shipSelectionPresenter;
        _mapPresenter = mapPresenter;
        _inGameMenuPresenter = inGameMenuPresenter;
    }
    
    public void Initialize()
    {
        ShowMainMenu();
        
        _shipSelectionPresenter
            .SetOnContinue(_mapPresenter.Show)
            .SetOnBack(ShowMainMenu);

        
        _inGameMenuPresenter
            .SetOnExit(_mapPresenter.Show);
    }
    
    private void ShowMainMenu()
    {
        _mainMenuPresenter
            .SetOnNewGame(StartNewGame)
            .SetOnContinue(ContinueGame)
            .Show();
    }
    
    private void StartNewGame()
    {
        _shipSelectionPresenter
            .Show();
        _mapPresenter
            .SetOnBack(_shipSelectionPresenter.Show);
    }
    
    private void ContinueGame()
    {
        _mapPresenter.Show();
        _mapPresenter
            .SetOnBack(ShowMainMenu);
    }
}
}