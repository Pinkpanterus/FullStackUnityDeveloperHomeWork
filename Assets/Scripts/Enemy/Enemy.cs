using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : Unit
    {
        [SerializeField] private AI_Component _aiComponent;

        
        public void SetDestination(Vector3 attackPositionPosition) => _aiComponent.SetDestination(attackPositionPosition);

        public void SetTarget(Unit target) => _aiComponent.SetTarget(target);

        public void SetAttackAllowance(bool isAllowed) => _aiComponent.SetAttackAllowance(isAllowed);
    }
}