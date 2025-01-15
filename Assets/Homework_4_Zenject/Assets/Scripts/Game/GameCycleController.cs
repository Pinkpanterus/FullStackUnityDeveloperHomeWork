using System;
using UnityEngine;
using Zenject;

public class GameCycleController: IInitializable, IDisposable
{
    public event Action OnGameStarted;
    public event Action<bool> OnGameOver;
    private readonly GameDifficultyController _gameDifficultyController;
    private readonly SnakeOutOfBoundariesController _snakeOutOfBoundariesController;
    private readonly SnakeSelfCollidedController _snakeSelfCollidedController;

    [Inject]
    public GameCycleController(GameDifficultyController gameDifficultyController, SnakeOutOfBoundariesController snakeOutOfBoundariesController, SnakeSelfCollidedController snakeSelfCollidedController)
    {
        _gameDifficultyController = gameDifficultyController;
        _snakeOutOfBoundariesController = snakeOutOfBoundariesController;
        _snakeSelfCollidedController = snakeSelfCollidedController;
    }

    public void Initialize()
    {
        OnGameStarted?.Invoke();
        _gameDifficultyController.OnMaxDifficultyReached += Win;
        _snakeOutOfBoundariesController.OnSnakeOutOfBoundaries += Loose;
        _snakeSelfCollidedController.OnSnakeSelfCollided += Loose;
    }

    public void Dispose()
    {
        _gameDifficultyController.OnMaxDifficultyReached -= Win;
        _snakeOutOfBoundariesController.OnSnakeOutOfBoundaries -= Loose;
        _snakeSelfCollidedController.OnSnakeSelfCollided -= Loose;
    }

    public void Win()
    {
        Debug.Log("Win");
        OnGameOver?.Invoke(true);
    }
    
    public void Loose()
    {
        Debug.unityLogger.Log("Loose");
        OnGameOver?.Invoke(false);
    }
}