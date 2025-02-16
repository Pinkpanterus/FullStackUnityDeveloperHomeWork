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
        
        private IPlanet _planet;
        private float _processTime;
        
        public PlanetPresenter(IPlanet planet)
        {
            _planet = planet;
            
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

        public IPlanet Planet => _planet;

        public void PlanetClick()
        {
            if (!_planet.IsUnlocked & _planet.CanUnlock)
                _planet.Unlock();

            if (_planet.IsIncomeReady)
                OnCoinPressed?.Invoke();
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

        public float IncomeProgress()
        {
            return _planet.IncomeProgress;
        }
        
        public string IncomeProgressAsString()
        {
            float timeInSeconds = _processTime - _planet.IncomeProgress * _processTime;
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);

            return string.Format("{0}m:{1:D2}s", minutes, seconds);
        }
        
        public string GetPrice()
        {
            return _planet.Price.ToString();
        }
    }
}