using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
    [SerializeField] private CaseSettings _caseSettings;
    [SerializeField] private FightSettings _fightSettings;
    
    public override void InstallBindings()
    {
        Container.Bind<CaseSettings>().FromInstance(_caseSettings);
        Container.Bind<FightSettings>().FromInstance(_fightSettings);
    }
}