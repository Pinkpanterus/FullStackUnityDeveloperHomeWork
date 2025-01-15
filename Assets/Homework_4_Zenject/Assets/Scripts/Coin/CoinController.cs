using System;
using System.Collections.Generic;
using System.Linq;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public sealed class CoinController: IInitializable, IDisposable
{
    public event Action OnAllCoinsCollected;
    public event Action<ICoin> OnSnakeGetCoin;
    public List<ICoin> Coins => _coins.ToList();
    
    private readonly ICoinSpawner _coinSpawner;
    private readonly IWorldBounds _worldBounds;
    private readonly IDifficulty _difficulty;
    
    private List<ICoin> _coins = new List<ICoin>();

    public CoinController(ICoinSpawner coinSpawner, IWorldBounds worldBounds, IDifficulty difficulty)
    {
        _coinSpawner = coinSpawner;
        _worldBounds = worldBounds;
        _difficulty = difficulty;
    }

    public void Initialize()
    {
        _difficulty.OnStateChanged += SpawnCoin;
    }

    public void Dispose()
    {
        _difficulty.OnStateChanged += SpawnCoin;
    }
    
    private void SpawnCoin()
    {
        int amount = _difficulty.Current;
        for (int i = 0; i < amount; i++)
        {
            Vector2Int randomPosition = _worldBounds.GetRandomPosition();
            ICoin coin = _coinSpawner.Spawn(randomPosition);
            _coins.Add(coin);
        }
    }

    public void SnakeGetCoin(ICoin coin)
    {
        _coinSpawner.Despawn(coin as Coin);
        OnSnakeGetCoin?.Invoke(coin);
        
        _coins.Remove(coin);
        if (_coins.Count == 0)
            OnAllCoinsCollected?.Invoke();
    }
}