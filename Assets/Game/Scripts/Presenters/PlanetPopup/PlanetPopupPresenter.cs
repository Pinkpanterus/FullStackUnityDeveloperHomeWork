using System;
using Modules.Planets;
using UnityEngine;

public class PlanetPopupPresenter
{
    public event Action<string> OnPopulationChanged;
    public event Action<string> OnLevelChanged;
    public event Action<string> OnIncomeChanged;
    public Sprite Icon => _planet.GetIcon(true);
    public string PlanetName => _planet.Name;
    public string Level => _planet.Level.ToString();
    public string Population => _planet.Population.ToString();
    public string MinuteIncome => _planet.MinuteIncome.ToString();
    public string Price => _planet.GetConfig().GetUpgradePrice(_planet.NextLevel).ToString();
    public bool CanUpgrade => _planet.CanUpgrade;

    private IPlanet _planet;

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

    private void PlanetOnIncomeChanged(int income)
    {
        OnIncomeChanged?.Invoke(income.ToString());
    }

    private void PlanetOnPopulationChanged(int population)
    {
        OnPopulationChanged?.Invoke(population.ToString());
    }

    public void UpgradePlanet()
    {
        _planet.Upgrade();
        OnLevelChanged?.Invoke(Level);
    }
}
