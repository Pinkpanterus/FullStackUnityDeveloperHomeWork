using Modules;
using UnityEngine;
using Zenject;


public sealed class CoinPool : MonoMemoryPool<Vector2, Coin>, ICoinSpawner
{
    protected override void Reinitialize(Vector2 position, Coin item)
    {
        item.transform.position = position;
        item.Generate();
    }

    // protected override void OnSpawned(Coin item)
    // {
    //     base.OnSpawned(item);
    //     item.OnDispose += this.Despawn;
    // }
    //
    // protected override void OnDespawned(Coin item)
    // {
    //     base.OnDespawned(item);
    //     item.OnDispose -= this.Despawn;
    // }

    ICoin ICoinSpawner.Spawn(Vector2 position)
    {
        return Spawn(position);
    }

    public void Despawn(Coin coin)
    {
        base.Despawn(coin);
    }
}
