using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class SquaresImageNoiseSettings
{
    [field: SerializeField]
    public Texture2D NoiseTexture { get; private set; }
}

[CreateAssetMenu(fileName = "Image Noise Installer", menuName = "CptLost/Installers/Image Noise")]
public class SquaresImageNoiseInstallerSO : ScriptableObjectInstaller
{
    [SerializeField]
    private SquaresImageNoiseSettings m_imageNoiseSettings;

    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(ITickable), typeof(ISquaresWeightGenerator), typeof(SquaresImageNoiseGenerator))
            .To<SquaresImageNoiseGenerator>()
            .AsSingle()
            .WithArguments(m_imageNoiseSettings);
    }
}
