using System;
using Modules;
using Zenject;

public sealed class SnakeStopController: IInitializable, IDisposable
{
    private readonly ISnake _snake;
    private readonly GameCycle _gameCycle;
    
    [Inject]
    public SnakeStopController(ISnake snake, GameCycle gameCycle)
    {
        _snake = snake;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _gameCycle.OnGameOver += StopSnake;
    }

    public void Dispose()
    {
        _gameCycle.OnGameOver += StopSnake;
    }

    private void StopSnake(bool win)
    {
        _snake.SetSpeed(0);
    }
}