using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class TextButton: Button
{
    [SerializeField]
    private Text _text;
    
    [SerializeField]
    private Image _background;

    public TextButton SetText(string text) 
    {
        _text.text = text;
        
        return this;
    }
    
    public TextButton SetColor(Color color)
    {
        _background.color = color;
        
        return this;
    }
}
}