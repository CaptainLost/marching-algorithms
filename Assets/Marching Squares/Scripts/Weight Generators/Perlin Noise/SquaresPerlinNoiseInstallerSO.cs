using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Perlin Noise Installer", menuName = "CptLost/Installers/Perlin Noise")]
public class SquaresPerlinNoiseInstallerSO : ScriptableObjectInstaller
{
    [SerializeField]
    private SquaresPerlinNoiseSettings m_perlinNoiseSettings;

    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(ITickable), typeof(ISquaresWeightGenerator))
            .To<SquaresPerlinNoiseGenerator>()
            .AsSingle()
            .WithArguments(m_perlinNoiseSettings);
    }
}