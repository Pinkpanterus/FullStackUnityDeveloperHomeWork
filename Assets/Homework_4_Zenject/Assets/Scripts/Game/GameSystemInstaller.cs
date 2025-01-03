using Zenject;

public class GameSystemInstaller: Installer<GameSystemInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameSystem>().AsSingle();
    }
}