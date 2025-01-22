using System;
using Modules;
using Zenject;

public class SnakeIncreaseSpeedController: IInitializable,IDisposable
{
    private readonly ISnake _snake;
    private readonly IDifficulty _gameDifficulty;
    
    [Inject]
    public SnakeIncreaseSpeedController(IDifficulty _difficulty, ISnake snake)
    {
        _gameDifficulty = _difficulty;
        _snake = snake;
    }

    public void Initialize()
    {
        _gameDifficulty.OnStateChanged += ChangeSpeed;
    }

    public void Dispose()
    {
        _gameDifficulty.OnStateChanged += ChangeSpeed;
    }

    private void ChangeSpeed()
    {
        int difficultyCurrent = _gameDifficulty.Current;
        _snake.SetSpeed(difficultyCurrent);
    }
}