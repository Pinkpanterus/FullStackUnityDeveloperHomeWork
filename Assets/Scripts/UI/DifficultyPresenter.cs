using System;
using Modules;
using SnakeGame;
using Zenject;

public class DifficultyPresenter: IInitializable, IDisposable
{
    private IDifficulty _difficulty;
    private IGameUI _gameUI;

    [Inject]
    public DifficultyPresenter(IGameUI gameUI, IDifficulty difficulty)
    {
        _gameUI = gameUI;
        _difficulty = difficulty;
    }

    public void Initialize()
    {
        UpdateDifficultyUI();
        _difficulty.OnStateChanged += UpdateDifficultyUI;
    }

    public void Dispose()
    {
        _difficulty.OnStateChanged -= UpdateDifficultyUI;
    }

    private void UpdateDifficultyUI()
    {   
        int difficultyCurrent = _difficulty.Current;
        int maxCurrent = _difficulty.Max;
        
        _gameUI.SetDifficulty(difficultyCurrent, maxCurrent);
    }
}