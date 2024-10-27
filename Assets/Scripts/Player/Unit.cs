using UnityEngine;

namespace ShootEmUp
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private AttackComponent _attackComponent;   
        
        public HealthComponent HealthComponent { get { return _healthComponent; } }
        public MoveComponent MoveComponent { get { return _moveComponent; } }
        public AttackComponent AttackComponent { get { return _attackComponent; } }
    }
}