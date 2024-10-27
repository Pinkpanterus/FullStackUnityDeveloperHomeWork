using UnityEngine;

namespace ShootEmUp
{
    [RequireComponent(typeof(AttackComponent))]
    public class AttackAgent : MonoBehaviour
    {
        [SerializeField] private float _attack_countdown = 1;
        [SerializeField] private AttackComponent _attackComponent;
        private float _currentTime;
        private bool _isAttackAllowed;
        private Unit _target;

        private void FixedUpdate()
        {
            if (_target == null)
                return;

            this._currentTime -= Time.fixedDeltaTime;
            if (this._currentTime <= 0)
            {
                // Vector2 startPosition = this._attackComponent.FirePoint.position;
                // Vector2 vector = (Vector2)this._target.transform.position - startPosition;
                // Vector2 direction = vector.normalized;
                // _attackComponent.Attack(direction);
                _attackComponent.AttackPosition((Vector2)_target.transform.position);

                this._currentTime += this._attack_countdown;
            }
        }

        // public void Reset()
        // {
        //     this._currentTime = this._attack_countdown;
        // }

        public void StartAttack(Unit target)
        {
            _target = target;
        }

        public void StopAttack()
        {
            _target = null;
        }
    }
}