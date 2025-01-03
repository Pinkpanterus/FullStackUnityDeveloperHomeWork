using System;
using System.Collections.Generic;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public sealed class GameSystem: IInitializable, IDisposable
{
    private readonly IDifficulty _difficulty;
    private readonly IScore _score;
    private readonly ISnake _snake;
    private readonly IWorldBounds _worldBounds;
    private readonly IGameUI _gameUI;
    private readonly ICoinSpawner _coinSpawner;
    private readonly GameConfig _gameConfig;
    
    private List<ICoin> _coins = new List<ICoin>();
    
    [Inject]
    public GameSystem(IDifficulty difficulty, IScore score, ISnake snake, IWorldBounds worldBounds, IGameUI gameUI, ICoinSpawner coinSpawner, GameConfig gameConfig)
    {
        _difficulty = difficulty;
        _score = score;
        _snake = snake;
        _worldBounds = worldBounds;
        _gameUI = gameUI;
        _coinSpawner = coinSpawner;
        _gameConfig = gameConfig;
    }


    public void Initialize()
    {
        _snake.OnMoved += OnSnakeMove;
        _snake.OnSelfCollided += GameOver;
        
        SwitchDifficulty();
        
        _gameUI.SetScore(_score.Current.ToString());
        _gameUI.SetDifficulty(_difficulty.Current,_gameConfig.LevelCount);
    }

    public void Dispose()
    {
        _snake.OnMoved -= OnSnakeMove;
        _snake.OnSelfCollided -= GameOver;
    }

    private void SwitchDifficulty()
    {
       if (_difficulty.Next(out int currentDifficulty))
       { 
           SpawnCoin(currentDifficulty);
           _snake.SetSpeed(1+currentDifficulty);
           
           _gameUI.SetDifficulty(currentDifficulty, 9);
       }
       else
           Win();
    }

    private void Win()
    {
        _snake.SetSpeed(0.0f);
        _gameUI.GameOver(true);
    }

    private void SpawnCoin(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2Int randomPosition = _worldBounds.GetRandomPosition();
            ICoin coin = _coinSpawner.Spawn(randomPosition);
            _coins.Add(coin);
        }
    }

    private void OnSnakeMove(Vector2Int position)
    {
        if (!_worldBounds.IsInBounds(position)) 
            GameOver();

        for (int i = 0; i < _coins.Count; i++)
        {
            Coin coin = _coins[i] as Coin;
            if (coin.Position == position)
            {
                PickUpCoin(coin);
            }
        }
    }

    private void PickUpCoin(Coin coin)
    {
        var score = coin.Score;
        _score.Add(score);
        _gameUI.SetScore(_score.Current.ToString());
                
        var bones = coin.Bones;
        _snake.Expand(bones);

        _coinSpawner.Despawn(coin);
        _coins.Remove(coin);
        if (_coins.Count == 0)
            SwitchDifficulty();
    }

    private void GameOver()
    {
        _snake.SetSpeed(0.0f);
        _gameUI.GameOver(false);
    }
}
