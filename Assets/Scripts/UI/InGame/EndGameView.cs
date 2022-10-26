using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class EndGameView: View
{
    [SerializeField]
    private GameObject _levelPassed;
    
    [SerializeField]
    private GameObject _levelFailed;
    
    [SerializeField]
    private Button _toLevelsButton;
    
    public EndGameView SetToLevels(Action action)
    {
        _toLevelsButton.onClick.AddListener(action.Invoke);
        
        return this;
    }
    
    public void ShowFail()
    {
        _levelFailed.SetActive(true);
        _levelPassed.SetActive(false);
        Show();
    }
    
    public void ShowSuccess()
    {
        _levelFailed.SetActive(false);
        _levelPassed.SetActive(true);
        Show();
    }
}
}