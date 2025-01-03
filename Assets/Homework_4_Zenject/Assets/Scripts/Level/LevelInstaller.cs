using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _coinPrefab;
    
    [SerializeField]
    private GameConfig _gameConfig;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WorldBounds>().FromInstance(FindObjectOfType<WorldBounds>()).AsSingle();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        CoinInstaller.Install(Container, _coinPrefab);
        GameSystemInstaller.Install(Container);
        ScoreInstaller.Install(Container);
        InputInstaller.Install(Container);
        Container.Bind<IDifficulty>().To<Difficulty>().FromMethod(_ => new Difficulty(_gameConfig.LevelCount)).AsSingle();
    }
}
