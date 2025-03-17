using System;
using Modules.Planets;
using UnityEngine;

public interface IPlanetPopupPresenter
{
    event Action<string> OnPopulationChanged;
    event Action<string> OnLevelChanged;
    event Action<string> OnIncomeChanged;
    Sprite Icon { get; }
    string PlanetName { get; }
    string Level { get; }
    string Population { get; }
    string MinuteIncome { get; }
    string Price { get; }
    bool CanUpgrade { get; }
    void ChangePlanet(IPlanet planet);
    void UpgradePlanet();
    void Hide();
}