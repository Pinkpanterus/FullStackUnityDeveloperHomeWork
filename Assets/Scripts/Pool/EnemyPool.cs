using System;

namespace ShootEmUp
{
    public class EnemyPool : Pool<Enemy>
    {
        // По идее эти события не нужны, но не знаю насколько правильно класс оставлять пустым

        // public event Action<Enemy> OnEnemySpawned;
        // public event Action<Enemy> OnEnemyDespawned;
        //
        // protected override void OnSpawned(Enemy enemy)
        // {
        //     OnEnemySpawned?.Invoke(enemy);
        // }
        //
        // protected override void OnDespawned(Enemy enemy)
        // {
        //     OnEnemyDespawned?.Invoke(enemy);
        // }
    }
}
