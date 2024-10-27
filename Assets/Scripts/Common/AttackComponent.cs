using UnityEngine;

namespace ShootEmUp
{
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _bulletSpeed = 2;
        [SerializeField] private PhysicsLayer _physicsLayer;
        [SerializeField] private Color _bulletColor;
        [SerializeField] private int _bulletDamage = 1;
        [SerializeField] private BulletManager _bulletManager;

        public Transform FirePoint
        {
            get { return _firePoint; }
        }

        public void SetBulletManager(BulletManager bulletManager)
        {
            _bulletManager = bulletManager;
        }


        public void Attack(Vector2 direction)
        {
            _bulletManager.SpawnBullet(_firePoint.position, _bulletColor, (int)_physicsLayer, _bulletDamage, _isPlayer, direction * _bulletSpeed);
        }
    }
}