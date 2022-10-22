using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShipSelectionView: View
{
    [SerializeField]
    private TextButton _buttonPrefab;
    
    [SerializeField]
    private Transform _buttonsContainer;
    
    [SerializeField]
    private ShipCharacteristicsView _characteristicsView;
    
    [SerializeField]
    private Button _backButton;
    
    [SerializeField]
    private Button _continueButton;
    
    [SerializeField]
    private Renderer _renderer;
    
    private Action _onContinue;
    private Action _onBack;

    private void Start()
    {
        _continueButton.onClick.AddListener(_onContinue.Invoke);
        _backButton.onClick.AddListener(_onBack.Invoke);
    }

    public void Draw(IList<string> names, Action<int> onSelect, int maxChar)
    {
        for (int i = 0; i < names.Count; i++)
        {
            var index = i;
            
            Instantiate(_buttonPrefab, _buttonsContainer)
                .SetText(names[index])
                .onClick.AddListener(() => onSelect.Invoke(index));
        }
        
        _characteristicsView.Init(maxChar);
    }
    
    public ShipSelectionView SetOnContinue(Action onContinue)
    {
        _onContinue = onContinue;
        
        return this;
    }
    
    public ShipSelectionView SetOnBack(Action onBack)
    {
        _onBack = onBack;
        
        return this;
    }
    
    public ShipSelectionView RepaintShip(Texture2D texture)
    {
        _renderer.material.mainTexture = texture;
        
        return this;
    }
    
    public ShipSelectionView SetCharacteristics(int health,
        int damage, int fireRate, int speed)
    {
        _characteristicsView.SetHealth(health);
        _characteristicsView.SetDamage(damage);
        _characteristicsView.SetFireRate(fireRate);
        _characteristicsView.SetSpeed(speed);
        
        return this;
    }
}