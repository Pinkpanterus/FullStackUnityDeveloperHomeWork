using Game.Presenters;
using UnityEngine;
using Zenject;

public class PlanetPopupInstaller : MonoInstaller
{
    [SerializeField]
    private PlanetPopup _planetPopup;
    public override void InstallBindings()
    {
        Container
            .Bind<PlanetPopup>()
            .FromInstance(_planetPopup)
            .AsSingle()
            .NonLazy();

    }
}
