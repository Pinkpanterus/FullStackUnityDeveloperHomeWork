using System.Collections;
using UnityEngine;

namespace ShootEmUp
{
    public class AI_Component : MonoBehaviour
    {
        [SerializeField] private MoveAgent _moveAgent;
        [SerializeField] private AttackAgent _attackAgent;
        private Unit _target;
        private bool _isAttackAllowed;
        private bool _isAttackInProgress;

        public void SetDestination(Vector2 endPoint)
        {
            _moveAgent.SetDestination(endPoint);
            _moveAgent.OnDestinationReached += StartAttack;
        }

        private void StartAttack()
        {
            _moveAgent.OnDestinationReached -= StartAttack;

            if (_target.GetCurrentHealth() > 0 && _isAttackAllowed && !_isAttackInProgress)
            {
                _attackAgent.StartAttack(_target);
                _isAttackInProgress = true;

                StartCoroutine(CheckAttackPossibility());
            }
        }

        private IEnumerator CheckAttackPossibility()
        {
            if (_target.GetCurrentHealth() <= 0 || !_isAttackAllowed || !_isAttackInProgress)
            {
                _attackAgent.StopAttack();
                _isAttackInProgress = false;
                
                yield break;
            }
            
            yield return null;
        }

        public void SetTarget(Unit target)
        {
            this._target = target;
        }

        public void SetAttackAllowance(bool isAllowed)
        {
            _isAttackAllowed = isAllowed;
        }
    }
}