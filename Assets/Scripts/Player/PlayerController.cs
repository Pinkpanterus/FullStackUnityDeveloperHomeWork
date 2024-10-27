using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Unit _player;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private BulletManager _bulletManager;
        
        private void Awake()
        {
            _player.OnDeath += RegisterPlayerDeath;
            _player.SetBulletManager(_bulletManager);
            
            _playerInput.OnFireButtonPressed += Attack;
            _playerInput.OnMoveButtonPressed += Move;
        }

        private void RegisterPlayerDeath(Unit unit)
        {
            _playerInput.OnFireButtonPressed -= Attack;
            _playerInput.OnMoveButtonPressed -= Move;
        }

        private void Attack()
        {
            _player.ForwardDirectionAttack();
        }

        private void Move(int vectorX)
        {
            Vector2 moveDirection = new Vector2(vectorX, 0);
            _player.Move(moveDirection);
        }
    }
}