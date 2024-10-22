using System;
using UnityEngine;

namespace ShootEmUp
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action<int> OnHealthPointsChanged;
        public event Action OnDeath;
        [SerializeField] private int _initialHealthPoints;
        private int _currentHealthPoints;

        private void Start()
        {
            _currentHealthPoints = _initialHealthPoints;
        }

        public void GetDamage(int damage)
        {
            _currentHealthPoints = Mathf.Max(0, _currentHealthPoints - damage);
            OnHealthPointsChanged?.Invoke(_currentHealthPoints);

            if (_currentHealthPoints <= 0)
                OnDeath?.Invoke();
        }

        public int GetCurrentHealth()
        {
            return _currentHealthPoints;
        }
    }
}