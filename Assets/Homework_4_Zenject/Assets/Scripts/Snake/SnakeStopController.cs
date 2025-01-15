using System;
using Modules;
using Zenject;

public class SnakeStopController: IInitializable, IDisposable
{
    private ISnake _snake;
    private GameCycleController gameCycleController;

    [Inject]
    public SnakeStopController(ISnake snake, GameCycleController gameCycleController)
    {
        _snake = snake;
        this.gameCycleController = gameCycleController;
    }

    public void Initialize()
    {
        gameCycleController.OnGameOver += SetSnakeSpeedToZero;
    }

    public void Dispose()
    {
        gameCycleController.OnGameOver -= SetSnakeSpeedToZero;
    }

    private void SetSnakeSpeedToZero(bool _)
    {
        _snake.SetSpeed(0);
    }
}