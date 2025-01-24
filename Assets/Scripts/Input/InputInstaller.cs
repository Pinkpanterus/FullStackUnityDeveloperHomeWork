using Modules;
using Zenject;

public sealed class InputInstaller : Installer<InputInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
    }
}
