using System;
using Modules;
using UnityEngine;
using Zenject;

public sealed class ScoreController: IInitializable, IDisposable
{
    private readonly IScore _score;
    private readonly CoinManager _coinManager;

    [Inject]
    public ScoreController(CoinManager coinManager, IScore score)
    {
        _coinManager = coinManager;
        _score = score;
    }

    public void Initialize()
    {
        _coinManager.OnCoinCollected += UpdateScore;
    }

    public void Dispose()
    {
        _coinManager.OnCoinCollected -= UpdateScore;
    }

    private void UpdateScore(ICoin coin)
    {
        _score.Add(coin.Score);
    }
}