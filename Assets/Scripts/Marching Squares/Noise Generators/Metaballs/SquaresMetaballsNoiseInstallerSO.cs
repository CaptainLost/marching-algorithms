using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class SquaresMetaballsNoiseSettings
{
    [field: SerializeField]
    public BallDisplay BallDisplayTemplate { get; private set; }
    [field: SerializeField]
    public int AmountOfBalls { get; private set; }
    [field: SerializeField]
    public float MinBallRadius { get; private set; }
    [field: SerializeField]
    public float MaxBallRadius { get; private set; }
    [field: SerializeField]
    public float MinBallVelocityX { get; private set; }
    [field: SerializeField]
    public float MaxBallVelocityX { get; private set; }
    [field: SerializeField]
    public float MinBallVelocityY { get; private set; }
    [field: SerializeField]
    public float MaxBallVelocityY { get; private set; }
}

[CreateAssetMenu(fileName = "Metaballs Installer", menuName = "CptLost/Installers/Metaballs")]
public class SquaresMetaballsNoiseInstallerSO : ScriptableObjectInstaller
{
    [field: SerializeField]
    public SquaresMetaballsNoiseSettings MetaballsSettings { get; private set; }

    public override void InstallBindings()
    {
        Container.Bind(typeof(IInitializable), typeof(ITickable), typeof(ISquaresNoiseGenerator))
            .To<SquaresMetaballsNoiseGenerator>()
            .AsSingle()
            .WithArguments(MetaballsSettings);
    }
}
