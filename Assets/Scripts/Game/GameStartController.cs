using UnityEngine;
using Zenject;

public sealed class GameStartController: IInitializable
{
    private readonly GameCycle _gameCycle;
    
    [Inject]
    public GameStartController(GameCycle gameCycle)
    {
        Debug.Log("Game Started");
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _gameCycle.StartGame();
    }
}