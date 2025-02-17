using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera worldCamera;
    [SerializeField]
    private Camera eyeCamera;
    [SerializeField]
    private Camera deviceCamera;
    [SerializeField]
    private GameObject deviceCameraViz;
    [SerializeField]
    private GameObject eyeCameraViz;
    [SerializeField]
    private ARCameraManager arCameraManager;

    public bool EyeCamUsed { get; private set; } = true;
    public bool DeviceCamUsed { get; private set; } = false;

    private void Start()
    {
        if (arCameraManager == null)
            arCameraManager = FindObjectOfType<ARCameraManager>();
            
        UpdateCameraState();
    }

    public void SetWorldCam()
    {
        EyeCamUsed = false;
        DeviceCamUsed = false;
        UpdateCameraState();
    }

    public void SetEyeCam()
    {
        EyeCamUsed = true;
        DeviceCamUsed = false;
        UpdateCameraState();
    }

    public void SetDeviceCam()
    {
        EyeCamUsed = false;
        DeviceCamUsed = true;
        UpdateCameraState();
    }

    private void UpdateCameraState()
    {
        worldCamera.gameObject.SetActive(!EyeCamUsed && !DeviceCamUsed);
        eyeCamera.enabled = EyeCamUsed;
        deviceCamera.enabled = DeviceCamUsed;
        deviceCameraViz.SetActive(!DeviceCamUsed);
        eyeCameraViz.SetActive(!EyeCamUsed);

        if (arCameraManager != null)
        {
            arCameraManager.enabled = DeviceCamUsed;
        }
    }
}