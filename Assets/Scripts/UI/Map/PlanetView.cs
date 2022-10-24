using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class PlanetView: MonoBehaviour
{
    [SerializeField]
    private Image _image;
    
    [SerializeField]
    private Button _button;
    
    public PlanetView SetColor(Color color)
    {
        _image.color = color;
        
        return this;
    }
    
    public PlanetView SetOnClick(Action onClick)
    {
        _button.onClick.AddListener(onClick.Invoke);
        
        return this;
    }
}
}