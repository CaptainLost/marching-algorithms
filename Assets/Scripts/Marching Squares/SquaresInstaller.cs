using UnityEngine;
using Zenject;

public class SquaresInstaller : MonoInstaller
{
    [SerializeField]
    private MarchingSquaresSettings m_squaresSettings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MarchingSquares>()
            .AsSingle()
            .WithArguments(m_squaresSettings);
    }
}
