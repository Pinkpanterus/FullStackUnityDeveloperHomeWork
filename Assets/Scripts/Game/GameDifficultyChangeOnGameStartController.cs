using System;
using Modules;
using UnityEngine;
using Zenject;

public sealed class GameDifficultyChangeOnGameStartController: IInitializable, IDisposable
{
    private readonly IDifficulty _difficulty;
    private readonly GameCycle _gameCycle;

    [Inject]
    public GameDifficultyChangeOnGameStartController(IDifficulty difficulty, GameCycle gameCycle)
    {
        _difficulty = difficulty;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _gameCycle.OnGameStarted += SwitchDifficulty;
    }

    public void Dispose()
    {
        _gameCycle.OnGameStarted -= SwitchDifficulty;
    }

    public void SwitchDifficulty()
    {
        _difficulty.Next(out int currentDifficulty);
        Debug.Log($"Switching difficulty at start to {currentDifficulty} level");
    }
}