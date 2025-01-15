using System;
using Modules;
using UnityEngine;
using Zenject;

public class ScoreController: IInitializable, IDisposable
{
    public event Action<int> OnScoreUpdated;
    private int _currentScore;
    private CoinController _coinController;

    [Inject]
    public ScoreController(CoinController coinController)
    {
        _coinController = coinController;
    }

    public void Initialize()
    {
        _coinController.OnSnakeGetCoin += UpdateScore;
    }

    public void Dispose()
    {
        _coinController.OnSnakeGetCoin -= UpdateScore;
    }

    private void UpdateScore(ICoin coin)
    {
        _currentScore += coin.Score;
        OnScoreUpdated?.Invoke(_currentScore);
    }

    public int CurrentScore => _currentScore;
}