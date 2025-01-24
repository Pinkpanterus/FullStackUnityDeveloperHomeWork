using System;
using SnakeGame;
using Zenject;

public sealed class EndGamePresenter: IInitializable, IDisposable
{
    private readonly GameCycle _gameCycle;
    private readonly IGameUI _gameUI;

    [Inject]
    public EndGamePresenter(GameCycle gameCycle, IGameUI gameUI)
    {
        _gameUI = gameUI;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _gameCycle.OnGameOver += ShowGameOverUI;
    }

    public void Dispose()
    {
        _gameCycle.OnGameOver -= ShowGameOverUI;
    }

    private void ShowGameOverUI(bool isSuccess)
    {
        _gameUI.GameOver(isSuccess);
    }
}
