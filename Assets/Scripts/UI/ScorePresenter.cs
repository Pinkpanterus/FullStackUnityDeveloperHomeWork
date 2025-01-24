using System;
using Modules;
using SnakeGame;
using Zenject;

public sealed class ScorePresenter: IInitializable, IDisposable
{
    private readonly IScore _score;
    private readonly IGameUI _gameUI;

    [Inject]
    public ScorePresenter(IGameUI gameUI, IScore score)
    {
        _gameUI = gameUI;
        _score = score;
    }

    public void Initialize()
    {
        UpdateScoreUI(_score.Current);
        _score.OnStateChanged += UpdateScoreUI;
    }

    public void Dispose()
    {
        _score.OnStateChanged -= UpdateScoreUI;
    }

    private void UpdateScoreUI(int currentScore)
    {
        _gameUI.SetScore(currentScore.ToString());
    }
}