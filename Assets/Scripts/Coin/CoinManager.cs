using System;
using System.Collections.Generic;
using Modules;
using SnakeGame;
using UnityEngine;

public sealed class CoinManager
{
    public event Action OnAllCoinsCollected;
    public event Action<ICoin> OnSnakeGetCoin;
    
    private readonly ICoinSpawner _coinSpawner;
    private readonly IWorldBounds _worldBounds;
    private readonly IDifficulty _difficulty;
    
    private List<ICoin> _coins = new List<ICoin>();

    public CoinManager(ICoinSpawner coinSpawner, IWorldBounds worldBounds, IDifficulty difficulty)
    {
        _coinSpawner = coinSpawner;
        _worldBounds = worldBounds;
        _difficulty = difficulty;
    }

    public void SpawnCoins()
    {
        int amount = _difficulty.Current;
        for (int i = 0; i < amount; i++)
        {
            Vector2Int randomPosition = _worldBounds.GetRandomPosition();
            ICoin coin = _coinSpawner.Spawn(randomPosition);
            _coins.Add(coin);
        }
    }

    private void SnakeGetCoin(ICoin coin)
    {
        _coinSpawner.Despawn(coin as Coin);
        OnSnakeGetCoin?.Invoke(coin);
        
        _coins.Remove(coin);
        
        if (_coins.Count == 0)
            OnAllCoinsCollected?.Invoke();
    }

    public void TryGetCoin(Vector2Int position)
    {
        ICoin[] coins = _coins.ToArray();
        foreach (ICoin coin in coins)
        {
            if (coin.Position == position) 
                SnakeGetCoin(coin);
        }
    }
}