using UnityEngine;

namespace ShootEmUp
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;

        private void Start()
        {
            _playerController.OnPlayerDeath += EndGame;
        }

        private void OnDestroy()
        {
            _playerController.OnPlayerDeath -= EndGame;
        }

        private void EndGame()
        {
            Debug.Log("Game Over");
            Time.timeScale = 0;
        }
    }
}