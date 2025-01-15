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
        Container.BindInterfacesAndSelfTo<WorldBounds>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        CoinInstaller.Install(Container, _coinPrefab);
        Container.BindInterfacesAndSelfTo<Difficulty>().FromNew().AsSingle().WithArguments(_gameConfig.LevelCount).NonLazy();
        GameSystemInstaller.Install(Container);
        ScoreInstaller.Install(Container);
        InputInstaller.Install(Container);
    }
}
