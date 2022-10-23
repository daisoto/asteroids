using Zenject;

namespace UI
{
public class UIManager: IInitializable
{
    private readonly MainMenuPresenter _mainMenuPresenter;
    private readonly ShipSelectionPresenter _shipSelectionPresenter;
    private readonly LevelSelectionPresenter _levelSelectionPresenter;
    private readonly InGameMenuPresenter _inGameMenuPresenter;

    public UIManager(MainMenuPresenter mainMenuPresenter, 
        ShipSelectionPresenter shipSelectionPresenter, 
        LevelSelectionPresenter levelSelectionPresenter, 
        InGameMenuPresenter inGameMenuPresenter)
    {
        _mainMenuPresenter = mainMenuPresenter;
        _shipSelectionPresenter = shipSelectionPresenter;
        _levelSelectionPresenter = levelSelectionPresenter;
        _inGameMenuPresenter = inGameMenuPresenter;
    }
    
    public void Initialize()
    {
        ShowMainMenu();
        
        _shipSelectionPresenter.Initialize(
            _levelSelectionPresenter.Show, ShowMainMenu);
        
        _inGameMenuPresenter.Initialize(_levelSelectionPresenter.Show);
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
}