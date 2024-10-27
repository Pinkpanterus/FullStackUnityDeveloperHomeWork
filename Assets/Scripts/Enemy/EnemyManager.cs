using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPositions;
        [SerializeField] private Transform[] attackPositions;
        [SerializeField] private Unit character;
        [SerializeField] private Transform worldTransform;
        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private BulletManager _bulletManager;
        private readonly HashSet<Enemy> m_activeEnemies = new();

     
        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));
        
                Enemy enemy = enemyPool.Rent();
                enemy.transform.SetParent(this.worldTransform);
                enemy.AttackComponent.SetBulletManager(_bulletManager);
        
                Transform spawnPosition = this.RandomPoint(this.spawnPositions);
                enemy.transform.position = spawnPosition.position;
        
                Transform attackPosition = this.RandomPoint(this.attackPositions);
                enemy.SetDestination(attackPosition.position);
                enemy.SetTarget(this.character);
        
                if (this.m_activeEnemies.Count < 5 && this.m_activeEnemies.Add(enemy))
                {
                    enemy.AIComponent.SetAttackAllow(true);
                }
            }
        }

        private void Fire(Vector2 position, Color _bulletcolor, int _physicslayer, int _bulletdamage, bool _isplayer, Vector2 direction)
        {
            _bulletManager.SpawnBullet(position, _bulletcolor, _physicslayer, _bulletdamage, _isplayer, direction);
        }

        private void FixedUpdate()
        {
            foreach (Enemy enemy in m_activeEnemies.ToArray())
            {
                if (enemy.HealthComponent.GetCurrentHealth() <= 0)
                {
                    enemy.AIComponent.SetAttackAllow(false);
                    enemy.transform.SetParent(this.enemyPool.transform);
                    
                    if (this.m_activeEnemies.Remove(enemy)) 
                    {
                        enemyPool.Return(enemy);
                    }
                }
            }
        }


        private Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}