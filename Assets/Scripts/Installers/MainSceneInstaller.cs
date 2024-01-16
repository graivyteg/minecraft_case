using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private ChestOpener _chestOpener;

        public override void InstallBindings()
        {
            Container.Bind<ChestOpener>().FromInstance(_chestOpener);
        }
    }
}