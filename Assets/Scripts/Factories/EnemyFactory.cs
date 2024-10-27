using ShootEmUp;
using UnityEngine;

namespace Factories.Configs
{
    public class EnemyFactory: MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private BulletManager _bulletManager;
        [SerializeField] private Transform _worldTransform;
        
        public Enemy Create()
        {
            Enemy enemy = _enemyPool.Rent();
            enemy.transform.SetParent(this._worldTransform);
            enemy.SetBulletManager(_bulletManager);
            enemy.OnDeath += Return;

            return enemy;
        }

        private void Return(Unit unit)
        {
            Enemy enemy = (Enemy)unit;
            enemy.OnDeath -= Return;
            enemy.transform.SetParent(_enemyPool.transform);
            _enemyPool.Return(enemy);
        }
    }
}