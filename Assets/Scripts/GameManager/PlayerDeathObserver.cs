using ShootEmUp;
using UnityEngine;

public class PlayerDeathObserver : MonoBehaviour
{
    [SerializeField] private Unit _player;
    [SerializeField] private GameCycle _gameCycle;

    private void OnEnable() => _player.OnDeath += _gameCycle.EndGame;
    private void OnDestroy() => _player.OnDeath -= _gameCycle.EndGame;
}
