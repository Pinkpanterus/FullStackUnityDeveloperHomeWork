using System;
using Game.Views;
using Modules.Planets;
using UnityEngine;
using Zenject;

public class PlanetPopupPresenter : IPlanetPopupPresenter
{
    public event Action<string> OnPopulationChanged;
    public event Action<string> OnLevelChanged;
    public event Action<string> OnIncomeChanged;
    public Sprite Icon => _planet.GetIcon(true);
    public string PlanetName => _planet.Name;
    public string Level => $"Level: {_planet.Level}"; 
    public string Population => $"Population: {_planet.Population}"; 
    public string MinuteIncome => $"Income: {_planet.MinuteIncome}"; 
    public string Price => _planet.IsMaxLevel? "Max level reached": _planet.GetConfig().GetUpgradePrice(_planet.NextLevel).ToString();
    public bool CanUpgrade => _planet.CanUpgrade;

    private IPlanet _planet;
    private IGameScreenPresenter _gameScreenPresenter;
    
    [Inject]
    public PlanetPopupPresenter(IGameScreenPresenter gameScreenPresenter)
    {
        _gameScreenPresenter = gameScreenPresenter;
    }


    private void PlanetOnIncomeChanged(int income)
    {
        OnIncomeChanged?.Invoke(MinuteIncome);
    }

    private void PlanetOnPopulationChanged(int population)
    {
        OnPopulationChanged?.Invoke(Population);
    }

    public void ChangePlanet(IPlanet planet)
    {
        if (_planet != null)
        {
            _planet.OnPopulationChanged -= PlanetOnPopulationChanged;
            _planet.OnIncomeChanged -= PlanetOnIncomeChanged;
        }
        
        _planet = planet;
        _planet.OnPopulationChanged += PlanetOnPopulationChanged;
        _planet.OnIncomeChanged += PlanetOnIncomeChanged;
    }

    public void UpgradePlanet()
    {
        _planet.Upgrade();
        OnLevelChanged?.Invoke(Level);
    }

    public void Hide()
    {
        _gameScreenPresenter.IsPopupVisible = false;
    }
}
