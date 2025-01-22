using System;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public sealed class SnakeOutOfBoundariesController: IInitializable, IDisposable
{
    private ISnake _snake;
    private IWorldBounds _worldBounds;
    private GameCycle _gameCycle;

    [Inject]
    public SnakeOutOfBoundariesController(ISnake snake, IWorldBounds worldBounds, GameCycle gameCycle)
    {
        _snake = snake;
        _worldBounds = worldBounds;
        _gameCycle = gameCycle;
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
            _gameCycle.EndGame(false);
    }
}