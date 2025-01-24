using System;
using Modules;
using UnityEngine;
using Zenject;

public sealed class CoinSpawnController: IInitializable, IDisposable
{
    private readonly CoinManager _coinManager;
    private readonly IDifficulty _difficulty;

    public CoinSpawnController(CoinManager coinManager, IDifficulty difficulty)
    {
        _coinManager = coinManager;
        _difficulty = difficulty;
    }

    public void Initialize()
    {
        _difficulty.OnStateChanged += SpawnCoins;
    }

    public void Dispose()
    {
        _difficulty.OnStateChanged -= SpawnCoins;
    }

    private void SpawnCoins()
    {
        Debug.Log("Spawning coins at difficulty change");
        _coinManager.SpawnCoins();
    }
}