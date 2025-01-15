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
    ICoin ICoinSpawner.Spawn(Vector2 position)
    {
        return Spawn(position);
    }
}
