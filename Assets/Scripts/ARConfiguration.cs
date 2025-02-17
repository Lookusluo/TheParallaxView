using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;

[CreateAssetMenu(fileName = "ARConfiguration", menuName = "AR/Configuration")]
public class ARConfiguration : ScriptableObject
{
    public void ConfigureAR()
    {
        // Configure AR Session
        var sessionOrigin = Object.FindObjectOfType<XROrigin>();
        if (sessionOrigin != null)
        {
            var cameraManager = sessionOrigin.Camera.GetComponent<ARCameraManager>();
            if (cameraManager != null)
            {
                ConfigureARCamera(cameraManager);
            }

            var faceManager = sessionOrigin.GetComponent<ARFaceManager>();
            if (faceManager == null)
            {
                // Add ARFaceManager if it doesn't exist
                faceManager = sessionOrigin.gameObject.AddComponent<ARFaceManager>();
            }
            
            if (faceManager != null)
            {
                ConfigureFaceTracking(faceManager);
            }
        }
        else
        {
            Debug.LogError("XR Origin not found in the scene. Please ensure it is properly set up.");
        }
    }

    private void ConfigureARCamera(ARCameraManager cameraManager)
    {
        // Configure camera resolution
        using (var configurations = cameraManager.GetConfigurations(Unity.Collections.Allocator.Temp))
        {
            if (configurations.Length > 0)
            {
                // Find the best landscape configuration
                var bestConfig = configurations[0];
                int maxResolution = 0;
                
                foreach (var config in configurations)
                {
                    // Prioritize configurations with landscape aspect ratio
                    if (config.resolution.x > config.resolution.y)
                    {
                        int resolution = config.resolution.x * config.resolution.y;
                        if (resolution > maxResolution)
                        {
                            maxResolution = resolution;
                            bestConfig = config;
                        }
                    }
                }
                
                cameraManager.currentConfiguration = bestConfig;
            }
        }

        // Configure screen settings for landscape
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        // Prevent screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void ConfigureFaceTracking(ARFaceManager faceManager)
    {
        faceManager.enabled = true;
        faceManager.requestedMaximumFaceCount = 1;
    }
}