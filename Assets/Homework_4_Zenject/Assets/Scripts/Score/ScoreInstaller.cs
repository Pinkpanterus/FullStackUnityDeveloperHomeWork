using Modules;
using Zenject;

public class ScoreInstaller : Installer<ScoreInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<Score>().AsSingle();
        Container.BindInterfacesAndSelfTo<ScoreController>().AsSingle();
    }
}
