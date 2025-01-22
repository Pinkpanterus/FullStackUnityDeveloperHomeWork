using System;
using Modules;
using Zenject;

public sealed class GameWinController: IInitializable,IDisposable
{
    private readonly IDifficulty _difficulty;
    private readonly GameCycle _gameCycle;
    private readonly CoinManager _coinManager;

    [Inject]
    public GameWinController(GameCycle gameCycle, CoinManager coinManager, IDifficulty difficulty)
    {
        _gameCycle = gameCycle;
        _coinManager = coinManager;
        _difficulty = difficulty;
    }

    public void Initialize()
    {
        _coinManager.OnAllCoinsCollected += CheckIfMaxLevel;
    }

    public void Dispose()
    {
        _coinManager.OnAllCoinsCollected += CheckIfMaxLevel;
    }

    private void CheckIfMaxLevel()
    {
        if (_difficulty.Current == _difficulty.Max)
            _gameCycle.EndGame(true);
    }
}