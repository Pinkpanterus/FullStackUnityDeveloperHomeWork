using System;
using Modules;
using Zenject;

public sealed class Difficulty: IInitializable, IDisposable
{
    private readonly IDifficulty _difficulty;
    private readonly CoinManager _coinManager;
    private readonly GameCycle _gameCycle;

    [Inject]
    public Difficulty(IDifficulty difficulty, CoinManager coinManager, GameCycle gameCycle)
    {
        _difficulty = difficulty;
        this._coinManager = coinManager;
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