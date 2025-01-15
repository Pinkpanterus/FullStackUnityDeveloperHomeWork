using System;
using SnakeGame;
using Zenject;

public sealed class UI_EndGameController: IInitializable, IDisposable
{
    private GameCycleController gameCycleController;
    private readonly IGameUI _gameUI;

    [Inject]
    public UI_EndGameController(GameCycleController gameCycleController, IGameUI gameUI)
    {
        this.gameCycleController = gameCycleController;
        _gameUI = gameUI;
    }

    public void Initialize()
    {
        gameCycleController.OnGameOver += ShowGameOverUI;
    }

    public void Dispose()
    {
        gameCycleController.OnGameOver -= ShowGameOverUI;
    }

    private void ShowGameOverUI(bool isSuccess)
    {
        _gameUI.GameOver(isSuccess);
    }
}
