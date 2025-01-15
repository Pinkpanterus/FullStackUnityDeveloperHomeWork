using System;
using Modules;
using Zenject;

public sealed class SnakeSelfCollidedController: IInitializable, IDisposable
{
    public event Action OnSnakeSelfCollided;
    private ISnake _snake;
    
    [Inject]
    public SnakeSelfCollidedController(ISnake snake)
    {
        _snake = snake;
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
        OnSnakeSelfCollided?.Invoke();
    }
}