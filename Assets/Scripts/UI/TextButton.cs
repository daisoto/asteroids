using UnityEngine;
using UnityEngine.UI;

public class TextButton: Button
{
    [SerializeField]
    private Text _text;
    
    public TextButton SetText(string text) 
    {
        _text.text = text;
        
        return this;
    }
}