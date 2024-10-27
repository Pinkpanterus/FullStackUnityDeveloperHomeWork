using System;
using UnityEngine;

namespace ShootEmUp
{
    [RequireComponent(typeof(HealthComponent), typeof(MoveComponent), typeof(AttackComponent))]
    public class Unit : MonoBehaviour
    {
        public event Action<Unit> OnDeath;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private AttackComponent _attackComponent;

        private void OnEnable()
        {
            _healthComponent.OnDeath += () => OnDeath?.Invoke(this);
        }

        public void ApplyDamage(int damage)
        {
            _healthComponent.ApplyDamage(damage);
        }

        public void SetBulletManager(BulletManager bulletManager)
        {
            _attackComponent.SetBulletManager(bulletManager);
        }

        public int GetCurrentHealth()
        {
            return _healthComponent.GetCurrentHealth();
        }

        public void Move(Vector2 moveDirection)
        {
            _moveComponent.Move(moveDirection);
        }

        public void ForwardDirectionAttack()
        {
            _attackComponent.ForwardDirectionAttack();
        }

        public void AttackPosition(Vector2 attackingPosition)
        {
            _attackComponent.AttackPosition(attackingPosition);
        }
    }
}