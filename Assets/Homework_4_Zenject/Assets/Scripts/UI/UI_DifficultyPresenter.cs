using System;
using SnakeGame;
using Zenject;

public class UI_DifficultyPresenter: IInitializable, IDisposable
{
    private GameDifficultyController _gameDifficultyController;
    private IGameUI _gameUI;

    [Inject]
    public UI_DifficultyPresenter(IGameUI gameUI, GameDifficultyController gameDifficultyController)
    {
        _gameUI = gameUI;
        _gameDifficultyController = gameDifficultyController;
    }

    public void Initialize()
    {
        UpdateDifficultyUI(_gameDifficultyController.CurrentDifficulty);
        _gameDifficultyController.OnDifficultyChanged += UpdateDifficultyUI;
    }

    public void Dispose()
    {
        _gameDifficultyController.OnDifficultyChanged -= UpdateDifficultyUI;
    }

    private void UpdateDifficultyUI(int currentDifficulty)
    {
        _gameUI.SetDifficulty(currentDifficulty, _gameDifficultyController.MaxDifficulty);
    }
}