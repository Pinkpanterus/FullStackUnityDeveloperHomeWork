using Modules;
using Zenject;

public class ScoreInstaller : Installer<ScoreInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IScore>().To<Score>().FromMethod(_=> new Score()).AsSingle();
    }
}
