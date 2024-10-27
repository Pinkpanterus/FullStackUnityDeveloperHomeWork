using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : Unit
    {
        [SerializeField] private AI_Component _aiComponent;

        public AI_Component AIComponent
        {
            get { return _aiComponent; }
        }


        public void SetDestination(Vector3 attackPositionPosition)
        {
            _aiComponent.SetDestination(attackPositionPosition);
        }

        public void SetTarget(Unit target)
        {
            _aiComponent.SetTarget(target);
        }
    }
}