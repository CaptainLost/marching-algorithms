using UnityEngine.Rendering.Universal;
using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class RaymarchingPassSettings
{
    public RenderPassEvent RenderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    public Shader Shader;
}

internal class RaymarchingRendererFeature : ScriptableRendererFeature
{
    [SerializeField]
    private RaymarchingPassSettings _passSettings = new();

    private RaymarchingPass _raymarchingPass;
    private Material _material;

    public override void Create()
    {
        _material = CoreUtils.CreateEngineMaterial(_passSettings.Shader);

        _raymarchingPass = new RaymarchingPass(_passSettings, _material);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _raymarchingPass.ConfigureInput(ScriptableRenderPassInput.Color);
        _raymarchingPass.SetTarget(renderer.cameraColorTargetHandle);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_raymarchingPass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_material);
    }
}