using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class MapView: View
{
    [SerializeField]
    private LevelPlanet[] _levelPlanets;
    [SerializeField]
    private Button _backButton;
    
    private IReadOnlyDictionary<int, LevelPlanet> _levelPlanetsDict => 
        _levelPlanetsDictInternal ??= 
            _levelPlanets.ToDictionary(lp => lp.Level, lp => lp);
    
    private Dictionary<int, LevelPlanet> _levelPlanetsDictInternal;

    public MapView SetOnBack(Action onBack)
    {
        _backButton.onClick.AddListener(onBack.Invoke);
        
        return this;
    }

    public void PaintPlanet(int level, Color color) => 
        _levelPlanetsDict[level].PlanetView.SetColor(color);
    
    public void SetOnClick(int level, Action onClick) => 
        _levelPlanetsDict[level].PlanetView.SetOnClick(onClick);
}

[Serializable]
public class LevelPlanet
{
    [SerializeField]
    private int _level;
    public int Level => _level;
    
    [SerializeField]
    private PlanetView _planetView;
    public PlanetView PlanetView => _planetView;
}
}