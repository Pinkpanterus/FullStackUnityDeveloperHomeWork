using System;
using Modules.Planets;
using UnityEngine;

namespace Game.Presenters
{
    public class PlanetPresenter: IDisposable, IPlanetPresenter
    {
        public event Action OnCoinPressed;
        public event Action OnPlanetGathered;
        public event Action<int> OnPlanetPopulationChanged;
        public event Action<int> OnPlanetUpgraded;
        public event Action<int> OnPlanetIncomeChanged;
        public event Action<float> OnPlanetIncomeTimeChanged;
        public event Action<bool> OnPlanetIncomeReady;
        public event Action OnPlanetUnlocked;
        
        private GameScreenPresenter _gameScreenPresenter;
        private PlanetPopupPresenter _planetPopupPresenter;
        private IPlanet _planet;
        private float _processTime;
        
        public PlanetPresenter(IPlanet planet, GameScreenPresenter gameScreenPresenter, PlanetPopupPresenter planetPopupPresenter)
        {
            _planet = planet;
            _gameScreenPresenter = gameScreenPresenter;
            _planetPopupPresenter = planetPopupPresenter;

            _planet.OnUnlocked += PlanetUnlock;
            _planet.OnUpgraded += PlanetUpgrade;
            _planet.OnIncomeChanged += PlanetIncomeChange;
            _planet.OnIncomeReady += PlanetIncomeReady;
            _planet.OnPopulationChanged += PlanetPopulationChange;
            _planet.OnIncomeTimeChanged += PlanetIncomeTimeChange;
            
            _processTime = _planet.GetConfig().IncomeDuration;
         }
      
        public void Dispose()
        {
            _planet.OnUnlocked -= PlanetUnlock;
            _planet.OnUpgraded -= PlanetUpgrade;
            _planet.OnIncomeChanged -= PlanetIncomeChange;
            _planet.OnIncomeReady -= PlanetIncomeReady;
            _planet.OnPopulationChanged -= PlanetPopulationChange;
            _planet.OnIncomeTimeChanged -= PlanetIncomeTimeChange;
        }

        private void PlanetIncomeTimeChange(float time)
        {
            OnPlanetIncomeTimeChanged?.Invoke(time);
        }

        private void PlanetPopulationChange(int population)
        {
            OnPlanetPopulationChanged?.Invoke(population);
        }

        private void PlanetIncomeReady(bool IsIncomeReady)
        {
            OnPlanetIncomeReady?.Invoke(IsIncomeReady);
        }

        private void PlanetIncomeChange(int minuteIncome)
        {
            OnPlanetIncomeChanged?.Invoke(minuteIncome);
        }

        private void PlanetUpgrade(int level)
        {
            OnPlanetUpgraded?.Invoke(level);
        }

        private void PlanetUnlock()
        {
            OnPlanetUnlocked?.Invoke();
        }

        private void PlanetGather()
        {
            OnPlanetGathered?.Invoke();
        }

        public void PlanetClick()
        {
            if (!_planet.IsUnlocked & _planet.CanUnlock)
                _planet.Unlock();

            if (_planet.IsIncomeReady)
                OnCoinPressed?.Invoke();
        }

        public void PlanetHold()
        {
            if (!_planet.IsUnlocked)
                return;
            
            _planetPopupPresenter.ChangePlanet(_planet);
            _gameScreenPresenter.IsPopupVisible = true;
        }

        public void PlanetIncomeGather()
        {
            _planet.GatherIncome();
        }

        public Sprite GetIcon(bool unlocked)
        {
            return _planet.GetIcon(unlocked);
        }
        
        public Sprite GetCurrentIcon()
        {
            return _planet.GetIcon(_planet.IsUnlocked);
        }

        public bool IsIncomeReady()
        {
            return _planet.IsIncomeReady;
        }

        public bool IsUnlocked()
        {
            return _planet.IsUnlocked;
        }

        public float IncomeProgress()
        {
            return _planet.IncomeProgress;
        }
        
        public string IncomeProgressAsString()
        {
            float timeInSeconds = _processTime - _planet.IncomeProgress * _processTime;
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);

            return $"{minutes}m:{seconds:D2}s";
        }
        
        public string GetPrice()
        {
            return _planet.Price.ToString();
        }
    }
}