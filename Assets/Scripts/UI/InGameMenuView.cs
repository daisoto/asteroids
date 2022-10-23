using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuView: View
{
    [SerializeField]
    private Button _continueButton;
    
    [SerializeField]
    private Button _exitButton;
    
    private void OnDestroy()
    {
        _continueButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
    
    public InGameMenuView OnContinue(Action onContinue)
    {
        _continueButton.onClick.AddListener(onContinue.Invoke);
        
        return this;
    }

    public InGameMenuView OnExit(Action onExit)
    {
        _exitButton.onClick.AddListener(onExit.Invoke);
        
        return this;
    }
}