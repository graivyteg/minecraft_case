using UnityEngine;
using Zenject;

public class FightSceneInstaller : MonoInstaller
{
    [SerializeField] private EnemySpawner _spawner;
    
    public override void InstallBindings()
    {
        Container.Bind<EnemySpawner>().FromInstance(_spawner);
    }
}
