namespace ShootEmUp
{
    public sealed class BulletPool : Pool<Bullet>
    {
        // По идее эти события не нужны, но не знаю насколько правильно класс оставлять пустым
        
        // public event Action<Bullet> OnBulletSpawned;
        // public event Action<Bullet> OnBulletDespawned;
        //
        // protected override void OnSpawned(Bullet bullet)
        // {
        //     OnBulletSpawned?.Invoke(bullet);
        // }
        //
        // protected override void OnDespawned(Bullet bullet)
        // {
        //     OnBulletDespawned?.Invoke(bullet);
        // }
    }
}