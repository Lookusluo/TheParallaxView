using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField]
    private Text fpsText;
    
    [SerializeField]
    private Text trackingStateText;
    
    [SerializeField]
    private ARSession arSession;
    
    [SerializeField]
    private ARFaceManager faceManager;

    private float deltaTime = 0.0f;

    private void Update()
    {
        UpdateFPS();
        UpdateTrackingState();
    }

    private void UpdateFPS()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {Mathf.Round(fps)}";
        }
    }

    private void UpdateTrackingState()
    {
        if (trackingStateText != null && faceManager != null)
        {
            string state = "No faces tracked";
            if (faceManager.trackables.count > 0)
            {
                foreach (ARFace face in faceManager.trackables)
                {
                    if (face.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
                    {
                        state = "Face Tracked";
                        break;
                    }
                }
            }
            trackingStateText.text = state;
        }
    }
}