using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Perlin Noise Installer", menuName = "CptLost/Installers/Perlin Noise")]
public class SquaresPerlinNoiseInstallerSO : ScriptableObjectInstaller
{
    [field: SerializeField]
    public SquaresPerlinNoiseSettings PerlinNoiseSettings { get; private set; }

    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(ITickable), typeof(ISquaresWeightGenerator))
            .To<SquaresPerlinNoiseGenerator>()
            .AsSingle()
            .WithArguments(PerlinNoiseSettings);
    }
}