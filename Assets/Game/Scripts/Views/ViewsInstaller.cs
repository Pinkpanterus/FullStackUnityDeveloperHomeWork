using System.Linq;
using Modules.UI;
using UnityEngine;
using Zenject;
using Game.MoneyWidget;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField] private MoneyView _moneyView;

        public override void InstallBindings()
        {
            GameObject[] planets = FindObjectsOfType<SmartButton>().Select(x => x.transform.parent.gameObject).ToArray();
            foreach (GameObject planet in planets)
            {
                Container.Bind<PlanetView>().FromNewComponentOn(planet).AsCached().NonLazy();
            }

            Container.BindInstance(_moneyView).AsSingle().NonLazy();
            // Container.Bind<Vector3>().FromMethod(() => _moneyView.transform.position).WhenInjectedInto<PlanetView>();
            Container.Bind<Vector3>().FromMethod(() => _moneyView.GetMoneyIconPosition()).WhenInjectedInto<PlanetView>();
            Container.Bind<ParticleAnimator>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}