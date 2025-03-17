using System;
using UnityEngine;

namespace Game.Presenters
{
    public interface IPlanetPresenter
    {
        event Action OnPlanetGathered;
        event Action<int> OnPlanetPopulationChanged;
        event Action<int> OnPlanetUpgraded;
        event Action<int> OnPlanetIncomeChanged;
        event Action<float> OnPlanetIncomeTimeChanged;
        event Action<bool> OnPlanetIncomeReady;
        event Action OnPlanetUnlocked;
        event Action OnCoinPressed;
        void PlanetClick();
        void PlanetHold();
        void PlanetIncomeGather();
        Sprite GetIcon(bool unlocked);
        Sprite GetCurrentIcon();
        bool IsIncomeReady();
        bool IsUnlocked();
        float IncomeProgress();
        string GetPrice();
        string IncomeProgressAsString();
    }
}