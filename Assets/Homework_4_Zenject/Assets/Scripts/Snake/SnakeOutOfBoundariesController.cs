using System;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public class SnakeOutOfBoundariesController: IInitializable, IDisposable
{
    public event Action OnSnakeOutOfBoundaries;
    private ISnake _snake;
    private IWorldBounds _worldBounds;

    [Inject]
    public SnakeOutOfBoundariesController(ISnake snake, IWorldBounds worldBounds)
    {
        _snake = snake;
        _worldBounds = worldBounds;
    }

    public void Initialize()
    {
        _snake.OnMoved += CheckForSnakeOutOfBoundaries;
    }

    public void Dispose()
    {
        _snake.OnMoved -= CheckForSnakeOutOfBoundaries;
    }

    private void CheckForSnakeOutOfBoundaries(Vector2Int pos)
    {
        if (!_worldBounds.IsInBounds(pos))
        {
            Debug.Log("Snake is out of bounds");
            OnSnakeOutOfBoundaries?.Invoke();
        }
    }
}