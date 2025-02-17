using UnityEngine;

[ExecuteInEditMode]
public class OffAxisProjection : MonoBehaviour
{
    public Camera deviceCamera;
    public Camera eyeCamera;
    public LineRenderer lineRenderer;
    public CameraManager camManager;

    public float left, right, bottom, top, near, far;
    public float nearDist;

    void LateUpdate()
    {
        if (!deviceCamera || !eyeCamera) return;

        // 更新相机朝向
        eyeCamera.transform.rotation = deviceCamera.transform.rotation * Quaternion.Euler(Vector3.up * 180);

        Vector3 deviceCamPos = eyeCamera.transform.worldToLocalMatrix.MultiplyPoint(deviceCamera.transform.position);
        Vector3 fwd = eyeCamera.transform.worldToLocalMatrix.MultiplyVector(deviceCamera.transform.forward);
        
        // 计算视锥体参数
        CalculateFrustumParameters(deviceCamPos, fwd);
        
        // 更新可视化
        UpdateFrustumVisualization(deviceCamPos);
        
        // 应用投影矩阵
        ApplyProjectionMatrix();
    }

    private void CalculateFrustumParameters(Vector3 deviceCamPos, Vector3 fwd)
    {
        // iPhone设备尺寸参数（单位：米）
        left = deviceCamPos.x - 0.000f;
        right = deviceCamPos.x + 0.135f;
        top = deviceCamPos.y + 0.022f;
        bottom = deviceCamPos.y - 0.040f;
        
        Plane device_plane = new Plane(fwd, deviceCamPos);
        Vector3 close = device_plane.ClosestPointOnPlane(Vector3.zero);
        near = close.magnitude;
        far = 10f;

        // 调整近平面
        float scale_factor = 0.01f / near;
        near *= scale_factor;
        left *= scale_factor;
        right *= scale_factor;
        top *= scale_factor;
        bottom *= scale_factor;
    }

    private void UpdateFrustumVisualization(Vector3 deviceCamPos)
    {
        if (lineRenderer != null && camManager != null)
        {
            lineRenderer.enabled = !camManager.EyeCamUsed;
            if (lineRenderer.enabled)
            {
                UpdateLineRendererPositions(deviceCamPos);
            }
        }
    }

    private void UpdateLineRendererPositions(Vector3 deviceCamPos)
    {
        if (lineRenderer != null)
        {
            Vector3[] positions = new Vector3[8];
            
            // Near plane corners
            positions[0] = new Vector3(left, bottom, near);
            positions[1] = new Vector3(right, bottom, near);
            positions[2] = new Vector3(right, top, near);
            positions[3] = new Vector3(left, top, near);
            positions[4] = positions[0];
            
            // Connect to device camera
            positions[5] = deviceCamPos;
            positions[6] = positions[2];
            positions[7] = positions[3];
    
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
    }

    private void ApplyProjectionMatrix()
    {
        eyeCamera.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, near, far);
    }

	static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float x = 2.0F * near / (right - left);
		float y = 2.0F * near / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x;
		m[0, 1] = 0;
		m[0, 2] = a;
		m[0, 3] = 0;
		m[1, 0] = 0;
		m[1, 1] = y;
		m[1, 2] = b;
		m[1, 3] = 0;
		m[2, 0] = 0;
		m[2, 1] = 0;
		m[2, 2] = c;
		m[2, 3] = d;
		m[3, 0] = 0;
		m[3, 1] = 0;
		m[3, 2] = e;
		m[3, 3] = 0;
		return m;
	}
}