using Zenject;

namespace UI
{
public class UIManager: IInitializable
{
    public const string SHIP_HEALTH_ID = "ship_health";
    
    private readonly MainMenuPresenter _mainMenuPresenter;
    private readonly ShipSelectionPresenter _shipSelectionPresenter;
    private readonly MapPresenter _mapPresenter;
    private readonly EndGamePresenter _endGamePresenter;
    
    public UIManager(MainMenuPresenter mainMenuPresenter, 
        ShipSelectionPresenter shipSelectionPresenter, 
        MapPresenter mapPresenter,  
        EndGamePresenter endGamePresenter)
    {
        _mainMenuPresenter = mainMenuPresenter;
        _shipSelectionPresenter = shipSelectionPresenter;
        _mapPresenter = mapPresenter;
        _endGamePresenter = endGamePresenter;
    }
    
    public void Initialize()
    {
        _mainMenuPresenter.Show();
        
        _mainMenuPresenter
            .SetOnNewGame(_shipSelectionPresenter.Show)
            .SetOnContinue(_mapPresenter.Show);
        
        _shipSelectionPresenter
            .SetOnContinue(_mapPresenter.Show)
            .SetOnBack(_mainMenuPresenter.Show);
        
        _mapPresenter
            .SetOnBack(_mainMenuPresenter.Show);
        
        _endGamePresenter
            .SetToLevels(_mapPresenter.Show);
    }
}
}