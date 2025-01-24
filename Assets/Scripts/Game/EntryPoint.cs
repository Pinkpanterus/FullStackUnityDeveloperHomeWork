using UnityEngine;
using Zenject;

public sealed class EntryPoint: MonoBehaviour
{
    [Inject]
    private readonly GameCycle _gameCycle;

    private void Start()
    {
        Debug.Log("Start");
        _gameCycle.StartGame();
    }
}