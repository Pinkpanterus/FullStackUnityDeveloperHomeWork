using System;
using System.Collections.Generic;
using Modules;
using UnityEngine;
using Zenject;

public sealed class SnakeGetCoinController: IInitializable, IDisposable
{
    private CoinController _coinController;
    private ISnake _snake;
    
    [Inject]
    public SnakeGetCoinController(CoinController coinController, ISnake snake)
    {
        _coinController = coinController;
        _snake = snake;
    }

    public void Initialize()
    {
        _snake.OnMoved += GetCoin;
    }

    public void Dispose()
    {
        _snake.OnMoved -= GetCoin;
    }

    private void GetCoin(Vector2Int position)
    {
        List<ICoin> coins = _coinController.Coins;
        foreach (ICoin coin in coins)
        {
            if (coin.Position == position)
            {
                _coinController.SnakeGetCoin(coin);
                _snake.Expand(coin.Bones);
            }
        }
    }
}