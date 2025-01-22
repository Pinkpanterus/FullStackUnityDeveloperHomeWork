using Zenject;

public sealed class GameSystemInstaller: Installer<GameSystemInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameWinController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GameDifficultyChangeOnGameStartController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GameDifficultyChangeOnAllCoinsCollectedController>().FromNew().AsSingle();
        Container.Bind<GameCycle>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStartController>().FromNew().AsSingle();
    }
}