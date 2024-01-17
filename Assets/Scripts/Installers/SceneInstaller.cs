using Audio;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private SoundController _soundController;
    [SerializeField] private EntityManager _entityManager;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Player>().FromNew().AsSingle().NonLazy();
        Container.Bind<EntityManager>().FromInstance(_entityManager);
        Container.Bind<SoundController>().FromInstance(_soundController);
    }
}