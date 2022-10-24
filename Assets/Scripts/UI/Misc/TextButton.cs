using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class TextButton: View
{
    [SerializeField]
    private Button _button;
    
    public Button Button => _button;
    
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