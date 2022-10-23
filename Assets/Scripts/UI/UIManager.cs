using Zenject;

public class UIManager: IInitializable
{
    private readonly MainMenuPresenter _mainMenuPresenter;
    private readonly ShipSelectionPresenter _shipSelectionPresenter;
    private readonly LevelSelectionPresenter _levelSelectionPresenter;

    public UIManager(MainMenuPresenter mainMenuPresenter, 
        ShipSelectionPresenter shipSelectionPresenter, 
        LevelSelectionPresenter levelSelectionPresenter)
    {
        _mainMenuPresenter = mainMenuPresenter;
        _shipSelectionPresenter = shipSelectionPresenter;
        _levelSelectionPresenter = levelSelectionPresenter;
    }
    
    public void Initialize()
    {
        ShowMainMenu();
        
        _shipSelectionPresenter.Initialize(
            _levelSelectionPresenter.Show, ShowMainMenu);
    }
    
    private void ShowMainMenu()
    {
        _mainMenuPresenter.
            Initialize(
            _shipSelectionPresenter.Show, 
            _levelSelectionPresenter.Show)
            .Show();
    }
}