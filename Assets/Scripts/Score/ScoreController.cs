using System;
using Modules;
using UnityEngine;
using Zenject;

public class ScoreController: IInitializable, IDisposable
{
    public event Action<int> OnScoreUpdated;
    private int _currentScore;
    private CoinManager coinManager;

    [Inject]
    public ScoreController(CoinManager coinManager)
    {
        this.coinManager = coinManager;
    }

    public void Initialize()
    {
        coinManager.OnSnakeGetCoin += UpdateScore;
    }

    public void Dispose()
    {
        coinManager.OnSnakeGetCoin -= UpdateScore;
    }

    private void UpdateScore(ICoin coin)
    {
        _currentScore += coin.Score;
        OnScoreUpdated?.Invoke(_currentScore);
    }

    public int CurrentScore => _currentScore;
}