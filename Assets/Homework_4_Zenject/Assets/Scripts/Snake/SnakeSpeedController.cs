using System;
using Modules;
using UnityEngine;
using Zenject;

public class SnakeSpeedController: IInitializable,IDisposable
{
    private ISnake _snake;
    private GameDifficultyController _gameDifficultyController;
    
    [Inject]
    public SnakeSpeedController(GameDifficultyController gameDifficultyController, ISnake snake)
    {
        _gameDifficultyController = gameDifficultyController;
        _snake = snake;
    }

    public void Initialize()
    {
        _gameDifficultyController.OnDifficultyChanged += ChangeSpeed;
        _gameDifficultyController.OnMaxDifficultyReached += StopSnake;
    }

    public void Dispose()
    {
        _gameDifficultyController.OnDifficultyChanged -= ChangeSpeed;
        _gameDifficultyController.OnMaxDifficultyReached -= StopSnake;
    }

    private void ChangeSpeed(int difficultyLevel)
    {
        _snake.SetSpeed(difficultyLevel);
    }

    private void StopSnake()
    {
        _snake.SetSpeed(0);
    }
}