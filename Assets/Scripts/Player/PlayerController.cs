using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        public Action OnPlayerDeath;
        [SerializeField] private Unit _player;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private BulletManager _bulletManager;
        
        private void Awake()
        {
            _player.HealthComponent.OnDeath += RegisterPlayerDeath;
            _player.AttackComponent.SetBulletManager(_bulletManager);
            
            _playerInput.OnFireButtonPressed += Attack;
            _playerInput.OnMoveButtonPressed += Move;
        }

        private void RegisterPlayerDeath()
        {
            OnPlayerDeath?.Invoke();
            
            _playerInput.OnFireButtonPressed -= Attack;
            _playerInput.OnMoveButtonPressed -= Move;
        }

        private void Attack()
        {
            _player.AttackComponent.Attack(_player.AttackComponent.FirePoint.rotation * Vector3.up);
        }

        private void Move(int vectorX)
        {
            Vector2 moveDirection = new Vector2(vectorX, 0);
            _player.MoveComponent.Move(moveDirection);
        }
    }
}