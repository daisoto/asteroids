using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView: View
{
    [SerializeField]
    private Button _newGameButton;
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _exitButton;
    
    private void OnDestroy()
    {
        _newGameButton.onClick.RemoveAllListeners();
        _continueButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
    
    public MainMenuView OnNewGame(Action onNewGame)
    {
        _newGameButton.onClick.AddListener(onNewGame.Invoke);
        
        return this;
    }
    
    public MainMenuView OnContinue(Action onContinue)
    {
        _continueButton.onClick.AddListener(onContinue.Invoke);
        
        return this;
    }
    
    public MainMenuView OnExit(Action onExit)
    {
        _exitButton.onClick.AddListener(onExit.Invoke);
        
        return this;
    }
    
    public MainMenuView SetContinue(bool flag)
    {
        _continueButton.gameObject.SetActive(flag);
        
        return this;
    }
}