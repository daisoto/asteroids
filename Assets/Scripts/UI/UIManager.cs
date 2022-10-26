using Zenject;

namespace UI
{
public class UIManager: IInitializable // TODO : to state machine?
{
    public const string SHIP_HEALTH_ID = "ship_health";
    
    private readonly MainMenuPresenter _mainMenuPresenter;
    private readonly ShipSelectionPresenter _shipSelectionPresenter;
    private readonly MapPresenter _mapPresenter;
    private readonly InGameMenuPresenter _inGameMenuPresenter;
    private readonly ShipHealthPresenter _shipHealthPresenter;
    private readonly EndGamePresenter _endGamePresenter;
    
    public UIManager(MainMenuPresenter mainMenuPresenter, 
        ShipSelectionPresenter shipSelectionPresenter, 
        MapPresenter mapPresenter, 
        InGameMenuPresenter inGameMenuPresenter, 
        ShipHealthPresenter shipHealthPresenter, 
        EndGamePresenter endGamePresenter)
    {
        _mainMenuPresenter = mainMenuPresenter;
        _shipSelectionPresenter = shipSelectionPresenter;
        _mapPresenter = mapPresenter;
        _inGameMenuPresenter = inGameMenuPresenter;
        _shipHealthPresenter = shipHealthPresenter;
        _endGamePresenter = endGamePresenter;
    }
    
    public void Initialize()
    {
        ShowMainMenu();
        
        _mainMenuPresenter
            .SetOnNewGame(StartNewGame)
            .SetOnContinue(ContinueGame);
        
        _shipSelectionPresenter
            .SetOnContinue(_mapPresenter.Show)
            .SetOnBack(ShowMainMenu);

        _inGameMenuPresenter
            .SetOnExit(ShowPlanetMenu);
        
        _mapPresenter
            .SetOnPlanetChose(ShowGameUI);
        
        _endGamePresenter
            .SetToLevels(ShowPlanetMenu)
            .SetOnShow(CloseInGameViews);
    }
    
    private void ShowMainMenu()
    {
        _mainMenuPresenter
            .Update()
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
    
    private void ShowGameUI()
    {
        _shipHealthPresenter.Show();
        _inGameMenuPresenter.Show();
    }
    
    private void ShowPlanetMenu()
    {
        CloseInGameViews();
        _endGamePresenter.Close();
        _mapPresenter.Show();
    }
    
    private void CloseInGameViews()
    {
        _shipHealthPresenter.Close();
        _inGameMenuPresenter.Close();
    }
}
}