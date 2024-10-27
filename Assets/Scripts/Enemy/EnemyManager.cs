using System;
using System.Collections;
using System.Collections.Generic;
using Factories.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;
        [SerializeField] private Unit _character; // Долго думал может перенести _character в EnemyFactory, но решил что здесь все же логичнее
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private TimerScript _timer;
        private readonly HashSet<Enemy> _m_activeEnemies = new();


        private void Start()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            float delay = Random.Range(1f, 2f);
            _timer.SetDuration(delay);
            _timer.Play();
            _timer.OnFinished += SpawnEnemy;
        }

        private void SpawnEnemy()
        {
            _timer.Cancel();
            _timer.OnFinished -= SpawnEnemy;

            Enemy enemy = _enemyFactory.Create();
            Configure(enemy);

            if (this._m_activeEnemies.Count < 5 && this._m_activeEnemies.Add(enemy))
            {
                enemy.SetAttackAllowance(true);
            }

            StartTimer();
        }


        private void Configure(Enemy enemy)
        {
            Transform spawnPosition = this.RandomPoint(this._spawnPositions);
            enemy.transform.position = spawnPosition.position;

            Transform attackPosition = this.RandomPoint(this._attackPositions);
            enemy.SetDestination(attackPosition.position);
            enemy.SetTarget(this._character);
            enemy.OnDeath += RegisterDeath;
        }

        private void RegisterDeath(Unit unit)
        {
            Enemy enemy = (Enemy)unit;
            enemy.SetAttackAllowance(false);
            this._m_activeEnemies.Remove(enemy);
        }

        private Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}