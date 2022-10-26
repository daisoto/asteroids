using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class InGameMenuView: View
{
    [SerializeField]
    private Button _openButton;
    
    [SerializeField]
    private Button _continueButton;
    
    [SerializeField]
    private Button _exitButton;
    
    [SerializeField]
    private GameObject _menu;
    
    private Action _onShowMenu;

    private void Start()
    {
        _openButton.onClick.AddListener(ShowMenu);
        _continueButton.onClick.AddListener(CloseMenu);
    }

    private void OnDestroy()
    {
        _continueButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
    
    public InGameMenuView SetOnShowMenu(Action onShowMenu)
    {
        _onShowMenu = onShowMenu;
        
        return this;
    }
    
    public InGameMenuView SetOnContinue(Action onContinue)
    {
        _continueButton.onClick.AddListener(onContinue.Invoke);
        
        return this;
    }

    public InGameMenuView SetOnExit(Action onExit)
    {
        _exitButton.onClick.AddListener(onExit.Invoke);
        
        return this;
    }
    
    public void CloseMenu()
    {
        _menu.SetActive(false);
        _openButton.gameObject.SetActive(true);
    }
    
    private void ShowMenu()
    {
        _onShowMenu?.Invoke();
        _menu.SetActive(true);
        _openButton.gameObject.SetActive(false);
    }
}
}