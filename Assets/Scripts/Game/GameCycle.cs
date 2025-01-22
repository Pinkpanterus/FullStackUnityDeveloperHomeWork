using System;

public class GameCycle
{
    public event Action OnGameStarted;
    public event Action<bool> OnGameOver;

    public void StartGame() => OnGameStarted?.Invoke();
    public void EndGame(bool result) => OnGameOver?.Invoke(result);
}