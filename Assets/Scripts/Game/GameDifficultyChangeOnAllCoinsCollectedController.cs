using System;
using Modules;
using Zenject;

public sealed class GameDifficultyChangeOnAllCoinsCollectedController: IInitializable, IDisposable
{
    private readonly IDifficulty _difficulty;
    private readonly CoinManager coinManager;

    [Inject]
    public GameDifficultyChangeOnAllCoinsCollectedController(IDifficulty difficulty, CoinManager coinManager)
    {
        _difficulty = difficulty;
        this.coinManager = coinManager;
    }

    public void Initialize()
    {
        coinManager.OnAllCoinsCollected += SwitchDifficulty;
    }

    public void Dispose()
    {
        coinManager.OnAllCoinsCollected -= SwitchDifficulty;
    }

    public void SwitchDifficulty()
    {
        _difficulty.Next(out int currentDifficulty);
    }
}