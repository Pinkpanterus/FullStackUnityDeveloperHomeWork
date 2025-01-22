using Modules;
using UnityEngine;

public interface ICoinSpawner
{
    ICoin Spawn(Vector2 position);
    void Despawn(Coin coin);
}