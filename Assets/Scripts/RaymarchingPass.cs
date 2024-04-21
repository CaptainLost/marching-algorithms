using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class RaymarchingPass : ScriptableRenderPass
{
    private RaymarchingPassSettings _passSettings;
    private Material _material;

    private ProfilingSampler _profilingSampler = new ProfilingSampler("Raymarching");
    private RTHandle _cameraColorTarget, _raymarchingTarget;
    private int _raymarchingBufferID = Shader.PropertyToID("_RaymarchingBuffer");

    // Test
    private RenderTargetIdentifier pixelBuffer;

    public RaymarchingPass(RaymarchingPassSettings passSettings, Material material)
    {
        _passSettings = passSettings;
        _material = material;

        renderPassEvent = _passSettings.RenderPassEvent;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(_cameraColorTarget);

        //RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        //cmd.GetTemporaryRT(_raymarchingBufferID, descriptor, FilterMode.Point);

        //pixelBuffer = new RenderTargetIdentifier(_raymarchingBufferID);
        //_raymarchingTarget = RTHandles.Alloc(_raymarchingBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_material == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get();

        using (new ProfilingScope(cmd, _profilingSampler))
        {
            //Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, 0);
            Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _material, 0);

            //Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _raymarchingTarget, _material, 0);
            //Blitter.BlitCameraTexture(cmd, _raymarchingTarget, _cameraColorTarget);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(_raymarchingBufferID);
    }

    public void SetTarget(RTHandle cameraColorTargetHandle)
    {
        _cameraColorTarget = cameraColorTargetHandle;
    }

    //public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    //{
    //    var cameraData = renderingData.cameraData;
    //    if (cameraData.camera.cameraType != CameraType.Game)
    //        return;

    //    if (m_Material == null)
    //        return;

    //    CommandBuffer cmd = CommandBufferPool.Get();
    //    using (new ProfilingScope(cmd, m_ProfilingSampler))
    //    {
    //        m_Material.SetFloat("_Intensity", m_Intensity);
    //        Blitter.BlitCameraTexture(cmd, m_CameraColorTarget, m_CameraColorTarget, m_Material, 0);
    //    }
    //    context.ExecuteCommandBuffer(cmd);
    //    cmd.Clear();

    //    CommandBufferPool.Release(cmd);
    //}
}