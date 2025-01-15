using System;
using UnityEngine;
using Zenject;

public sealed class GameStartController: IInitializable, IDisposable
{
    private readonly GameCycleController _gameCycleController;
    private readonly GameDifficultyController _gameDifficultyController;
    
    [Inject]
    public GameStartController(GameCycleController gameCycleController, GameDifficultyController gameDifficultyController)
    {
        _gameCycleController = gameCycleController;
        _gameDifficultyController = gameDifficultyController;
    }

    public void Initialize()
    {
        _gameCycleController.OnGameStarted += ChangeDifficulty;
    }

    public void Dispose()
    {
        _gameCycleController.OnGameStarted += ChangeDifficulty;
    }

    private void ChangeDifficulty()
    {
        _gameDifficultyController.SwitchDifficulty();
    }
}