using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Perlin Noise Installer", menuName = "CptLost/Installers/Perlin Noise")]
public class SquaresPerlinNoiseInstallerSO : ScriptableObjectInstaller<SquaresPerlinNoiseInstallerSO>
{
    [field: SerializeField]
    public SquaresPerlinNoiseSettings NoiseSettings { get; private set; }

    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(SquaresNoiseGenerator))
            .To<SquaresPerlinNoiseGenerator>()
            .AsSingle()
            .WithArguments(NoiseSettings);
    }
}