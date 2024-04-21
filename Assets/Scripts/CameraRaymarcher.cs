using UnityEngine;

public class CameraRaymarcher : MonoBehaviour
{
    [SerializeField]
    private Material _raymarchMaterial;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Matrix4x4 cameraFrustum = CalculateCameraFrustum(_camera);

        _raymarchMaterial.SetMatrix("_CamFrustum", cameraFrustum);
        _raymarchMaterial.SetMatrix("_CamToWorld", _camera.cameraToWorldMatrix);

        _raymarchMaterial.SetVector("_CamWorldSpace", _camera.transform.position);
    }

    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    Debug.Log("Draw");

    //    if (_raymarchMaterial == null)
    //    {
    //        Debug.Log("Draw def");

    //        Graphics.Blit(source, destination);

    //        return;
    //    }

    //    Matrix4x4 cameraFrustum = CalculateCameraFrustum(_camera);

    //    _raymarchMaterial.SetMatrix("_CamFrustum", cameraFrustum);
    //    _raymarchMaterial.SetMatrix("_CamToWorld", _camera.cameraToWorldMatrix);

    //    _raymarchMaterial.SetVector("_CamWorldSpace", _camera.transform.position);

    //    RenderTexture.active = destination;

    //    GL.PushMatrix();
    //    GL.LoadOrtho();

    //    _raymarchMaterial.SetPass(0);

    //    GL.Begin(GL.QUADS);

    //    // Bottom left
    //    GL.MultiTexCoord2(0, 0.0f, 0.0f);
    //    GL.Vertex3(0.0f, 0.0f, 3.0f);

    //    // Bottom right
    //    GL.MultiTexCoord2(0, 1.0f, 0.0f);
    //    GL.Vertex3(1.0f, 0.0f, 2.0f);

    //    // Top right
    //    GL.MultiTexCoord2(0, 1.0f, 1.0f);
    //    GL.Vertex3(1.0f, 1.0f, 1.0f);

    //    // Top left
    //    GL.MultiTexCoord2(0, 0.0f, 1.0f);
    //    GL.Vertex3(0.0f, 1.0f, 0.0f);

    //    GL.End();
    //    GL.PopMatrix();
    //}

    private Matrix4x4 CalculateCameraFrustum(Camera cam)
    {
        Matrix4x4 frustum = Matrix4x4.identity;

        float fov = Mathf.Tan((cam.fieldOfView * 0.5f) * Mathf.Deg2Rad);

        Vector3 goUp = Vector3.up * fov;
        Vector3 goRight = Vector3.right * fov * cam.aspect;

        Vector3 topLeftCorner = (-Vector3.forward - goRight + goUp);
        Vector3 topRightCorner = (-Vector3.forward + goRight + goUp);

        Vector3 bottomLeftCorner = (-Vector3.forward - goRight - goUp);
        Vector3 bottomRightCorner = (-Vector3.forward + goRight - goUp);

        frustum.SetRow(0, topLeftCorner);
        frustum.SetRow(1, topRightCorner);
        frustum.SetRow(2, bottomLeftCorner);
        frustum.SetRow(3, bottomRightCorner);

        return frustum;
    }
}
