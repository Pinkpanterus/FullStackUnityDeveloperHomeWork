using System;
using Modules;
using Zenject;

public sealed class GameDifficultyController: IInitializable, IDisposable
{
    public event Action<int> OnDifficultyChanged;
    public event Action OnMaxDifficultyReached;
    // private int _currentDifficulty;
    private readonly IDifficulty _difficulty;
    private readonly CoinController _coinController;

    [Inject]
    public GameDifficultyController(IDifficulty difficulty, CoinController coinController)
    {
        _difficulty = difficulty;
        _coinController = coinController;
    }

    public void Initialize()
    {
        _coinController.OnAllCoinsCollected += SwitchDifficulty;
    }

    public void Dispose()
    {
        _coinController.OnAllCoinsCollected -= SwitchDifficulty;
    }

    public void SwitchDifficulty()
    {
        if (_difficulty.Next(out int currentDifficulty))
        {
            OnDifficultyChanged?.Invoke(currentDifficulty);
        }
        else
        {
            OnMaxDifficultyReached?.Invoke();
        }
    }

    public int CurrentDifficulty => _difficulty.Current;
    public int MaxDifficulty => _difficulty.Max;
}