using CptLost.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MetaballsDisplayController : BaseSquaresDisplay
{
    [SerializeField]
    private MetaballDisplay m_ballDisplayTemplate;

    [Inject]
    private SquaresMetaballsNoiseGenerator m_metaballsNoise;

    private Dictionary<BallSimulation.Ball, MetaballDisplay> m_displayDictionary = new();
    private ComponentPool<MetaballDisplay> m_ballDisplayPool;

    private void Awake()
    {
        m_ballDisplayPool = new ComponentPool<MetaballDisplay>(m_ballDisplayTemplate, transform);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        m_metaballsNoise.BallSimulation.OnBallAdded += OnBallAdded;
        m_metaballsNoise.BallSimulation.OnBallRemoved += OnBallRemoved;

        UpdateBallDisplays();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        m_metaballsNoise.BallSimulation.OnBallAdded -= OnBallAdded;
        m_metaballsNoise.BallSimulation.OnBallRemoved -= OnBallRemoved;
    }

    public override void Generate()
    {
        UpdateBallDisplays();
    }

    // No safe checks needed for now
    private void OnBallAdded(BallSimulation.Ball ball)
    {
        MetaballDisplay ballDisplay = m_ballDisplayPool.DequeueObject();
        ballDisplay.Initialize(ball);

        m_displayDictionary.Add(ball, ballDisplay);
    }

    // No safe checks needed for now
    private void OnBallRemoved(BallSimulation.Ball ball)
    {
        MetaballDisplay ballDisplay = m_displayDictionary[ball];
        m_ballDisplayPool.EnqueueObject(ballDisplay);

        m_displayDictionary.Remove(ball);
    }

    private void UpdateBallDisplays()
    {
        foreach (KeyValuePair<BallSimulation.Ball, MetaballDisplay> display in m_displayDictionary)
        {
            display.Value.UpdateBall(display.Key);
        }
    }
}
