using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public sealed class GameConfig : ScriptableObject
{
    [SerializeField] private int _levelCount = 9;
    
    public int LevelCount => _levelCount;
}
