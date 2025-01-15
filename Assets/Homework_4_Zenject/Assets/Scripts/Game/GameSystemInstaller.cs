using Zenject;

public sealed class GameSystemInstaller: Installer<GameSystemInstaller>
{
    public override void InstallBindings()
    {
        // Container.BindInterfacesAndSelfTo<GameSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStartController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GameDifficultyController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GameCycleController>().FromNew().AsSingle();
    }
}