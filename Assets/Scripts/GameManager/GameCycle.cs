using UnityEngine;

namespace ShootEmUp
{
    public sealed class GameCycle : MonoBehaviour
    {
        public void EndGame(Unit player)
        {
            Debug.Log("Game Over");
            Time.timeScale = 0;
        }
    }
}