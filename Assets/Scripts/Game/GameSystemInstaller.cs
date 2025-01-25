using Zenject;

public sealed class GameSystemInstaller: Installer<GameSystemInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameWinController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<DifficultyController>().FromNew().AsSingle();
        Container.Bind<GameCycle>().FromNew().AsSingle();
        Container.Bind<EntryPoint>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }
}