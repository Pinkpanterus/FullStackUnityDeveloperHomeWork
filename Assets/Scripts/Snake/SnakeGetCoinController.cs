using System;
using Modules;
using UnityEngine;
using Zenject;

public sealed class SnakeGetCoinController: IInitializable, IDisposable
{
    private readonly CoinManager _coinManager;
    private readonly ISnake _snake;
    
    [Inject]
    public SnakeGetCoinController(CoinManager coinManager, ISnake snake)
    {
        _coinManager = coinManager;
        _snake = snake;
    }

    public void Initialize()
    {
        _snake.OnMoved += TryGetCoin;
    }

    public void Dispose()
    {
        _snake.OnMoved -= TryGetCoin;
    }

    private void TryGetCoin(Vector2Int position)
    {
        _coinManager.TryCollectCoin(position);
    }
}