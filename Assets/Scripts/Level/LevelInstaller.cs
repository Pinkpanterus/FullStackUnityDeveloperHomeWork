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
        Container.BindInterfacesAndSelfTo<Modules.Difficulty>().FromNew().AsSingle().WithArguments(_gameConfig.LevelCount).NonLazy();
        CoinInstaller.Install(Container, _coinPrefab);
        GameSystemInstaller.Install(Container);
        ScoreInstaller.Install(Container);
        InputInstaller.Install(Container);
    }
}
