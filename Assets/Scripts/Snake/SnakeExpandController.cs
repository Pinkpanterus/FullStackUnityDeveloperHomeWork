using System;
using Modules;
using Zenject;

public sealed class SnakeExpandController: IInitializable, IDisposable
{
    private readonly ISnake _snake;
    private readonly CoinManager _coinManager;

    public SnakeExpandController(ISnake snake, CoinManager coinManager)
    {
        _snake = snake;
        _coinManager = coinManager;
    }

    public void Initialize()
    {
        _coinManager.OnCoinCollected += ExpandSnake;
    }

    public void Dispose()
    {
        _coinManager.OnCoinCollected -= ExpandSnake;
    }

    private void ExpandSnake(ICoin coin)
    {
        _snake.Expand(coin.Bones);
    }
}