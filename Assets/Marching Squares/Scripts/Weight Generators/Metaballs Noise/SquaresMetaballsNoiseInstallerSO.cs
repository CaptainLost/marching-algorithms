using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Metaballs Noise Installer", menuName = "CptLost/Installers/Metaballs Noise")]
public class SquaresMetaballsNoiseInstallerSO : ScriptableObjectInstaller
{
    [SerializeField]
    private SquaresMetaballsNoiseSettings m_metaballsNoiseSettings;

    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(ITickable), typeof(ISquaresWeightGenerator), typeof(SquaresMetaballsNoiseGenerator))
            .To<SquaresMetaballsNoiseGenerator>()
            .AsSingle()
            .WithArguments(m_metaballsNoiseSettings);
    }
}
