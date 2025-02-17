using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    private Camera trackedCamera;
    [SerializeField]
    private XROrigin xrOrigin;
    private ARCameraManager cameraManager;
    private ARSession arSession;
    private bool sessionStarted = false;

    void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = FindObjectOfType<XROrigin>();
        }
        
        if (xrOrigin != null)
        {
            cameraManager = xrOrigin.Camera.GetComponent<ARCameraManager>();
            arSession = FindObjectOfType<ARSession>();
        }
        
        if (cameraManager != null)
        {
            cameraManager.frameReceived += OnFrameReceived;
        }
    }

    void OnDestroy()
    {
        if (cameraManager != null)
        {
            cameraManager.frameReceived -= OnFrameReceived;
        }
    }

    private void OnFrameReceived(ARCameraFrameEventArgs args)
    {
        if (!sessionStarted && ARSession.state == ARSessionState.SessionTracking)
        {
            sessionStarted = true;
        }

        if (trackedCamera != null && args.projectionMatrix.HasValue)
        {
            trackedCamera.projectionMatrix = args.projectionMatrix.Value;
        }
    }

    void Update()
    {
        if (trackedCamera != null && sessionStarted && xrOrigin != null)
        {
            var arCamera = xrOrigin.Camera;
            if (arCamera != null)
            {
                trackedCamera.transform.position = arCamera.transform.position;
                trackedCamera.transform.rotation = arCamera.transform.rotation;
            }
        }
    }
}