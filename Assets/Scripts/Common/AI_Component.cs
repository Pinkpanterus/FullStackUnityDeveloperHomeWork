using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public class AI_Component : MonoBehaviour
    {
        [SerializeField] private float _attack_countdown;
        [SerializeField] private Unit _target;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private AttackComponent _attackComponent;
        private Vector2 _destination;
        private float _currentTime;
        private bool _isPointReached;
        private bool _isAttackAllowed;

        private void OnEnable()
        {
            _isPointReached = false;
            _isAttackAllowed = false;
        }

        public void Reset()
        {
            this._currentTime = this._attack_countdown;
        }

        public void SetDestination(Vector2 endPoint)
        {
            this._destination = endPoint;
            this._isPointReached = false;
        }

        public void SetTarget(Unit target)
        {
            this._target = target;
        }

        public void SetAttackAllow(bool isAllowed)
        {
            _isAttackAllowed = isAllowed;
        }

        private void FixedUpdate()
        {
            if (this._isPointReached)
            {
                //Attack:
                if (_target.HealthComponent?.GetCurrentHealth() <= 0 && _isAttackAllowed)
                    return;

                this._currentTime -= Time.fixedDeltaTime;
                if (this._currentTime <= 0 && _target != null)
                {
                    Vector2 startPosition = this._attackComponent.FirePoint.position;
                    Vector2 vector = (Vector2)this._target.transform.position - startPosition;
                    Vector2 direction = vector.normalized;
                    _attackComponent.Attack(direction);

                    this._currentTime += this._attack_countdown;
                }
            }
            else
            {
                //Move:
                Vector2 vector = this._destination - (Vector2)this.transform.position;
                if (vector.magnitude <= 0.25f)
                {
                    this._isPointReached = true;
                    return;
                }

                Vector2 direction = vector.normalized;
                _moveComponent.Move(direction);
            }
        }
    }
}