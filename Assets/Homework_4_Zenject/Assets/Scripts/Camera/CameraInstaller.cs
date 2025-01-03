using Zenject;
using UnityEngine;

public class CameraInstaller: MonoInstaller<CameraInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(FindObjectOfType<Camera>()).AsSingle();
    }
}