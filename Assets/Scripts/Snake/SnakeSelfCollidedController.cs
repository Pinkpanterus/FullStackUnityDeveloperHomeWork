using System;
using Modules;
using Zenject;

public sealed class SnakeSelfCollidedController: IInitializable, IDisposable
{
    private readonly GameCycle _gameCycle;
    private readonly ISnake _snake;
    
    [Inject]
    public SnakeSelfCollidedController(ISnake snake, GameCycle gameCycle)
    {
        _snake = snake;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _snake.OnSelfCollided += RaiseEvent;
    }

    public void Dispose()
    {
        _snake.OnSelfCollided -= RaiseEvent;
    }

    private void RaiseEvent()
    {
        _gameCycle.EndGame(false);
    }
}