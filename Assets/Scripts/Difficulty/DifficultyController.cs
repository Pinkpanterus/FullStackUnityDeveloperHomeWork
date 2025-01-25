using System;
using Modules;
using Zenject;

public sealed class DifficultyController: IInitializable, IDisposable
{
    private readonly IDifficulty _difficulty;
    private readonly CoinManager _coinManager;
    private readonly GameCycle _gameCycle;

    [Inject]
    public DifficultyController(IDifficulty difficulty, CoinManager coinManager, GameCycle gameCycle)
    {
        _difficulty = difficulty;
        _coinManager = coinManager;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _coinManager.OnAllCoinsCollected += SwitchDifficulty;
        _gameCycle.OnGameStarted += SwitchDifficulty;
    }

    public void Dispose()
    {
        _coinManager.OnAllCoinsCollected -= SwitchDifficulty;
        _gameCycle.OnGameStarted -= SwitchDifficulty;
    }

    public void SwitchDifficulty()
    {
        _difficulty.Next(out int currentDifficulty);
    }
}