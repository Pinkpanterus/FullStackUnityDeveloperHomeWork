using System;
using SnakeGame;
using Zenject;

public class ScorePresenter: IInitializable, IDisposable
{
    private ScoreController _scoreController;
    private IGameUI _gameUI;

    [Inject]
    public ScorePresenter(IGameUI gameUI, ScoreController scoreController)
    {
        _gameUI = gameUI;
        _scoreController = scoreController;
    }

    public void Initialize()
    {
        UpdateScoreUI(_scoreController.CurrentScore);
        _scoreController.OnScoreUpdated += UpdateScoreUI;
    }

    public void Dispose()
    {
        _scoreController.OnScoreUpdated -= UpdateScoreUI;
    }

    private void UpdateScoreUI(int currentScore)
    {
        _gameUI.SetScore(currentScore.ToString());
    }
}