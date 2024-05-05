using UnityEngine;
using Zenject;

public class MarchingSquaresInstaller : MonoInstaller
{
    [SerializeField]
    private MarchingSquaresSettings m_settings;

    public override void InstallBindings()
    {
        Container.Bind<MarchingSquares>()
            .AsSingle()
            .WithArguments(m_settings);
    }
}
