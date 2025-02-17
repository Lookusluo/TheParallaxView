using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Management;
using UnityEngine.XR.ARSubsystems;

[CreateAssetMenu(fileName = "ARConfiguration", menuName = "AR/Configuration")]
public class ARConfiguration : ScriptableObject
{
    public void ConfigureAR()
    {
        var generalSettings = XRGeneralSettings.Instance;
        if (generalSettings != null)
        {
            var manager = generalSettings.Manager;
            if (manager != null)
            {
                manager.InitializeLoaderSync();
                var loader = manager.activeLoader as ARKitLoader;
                if (loader != null)
                {
                    var subsystem = loader.GetLoadedSubsystem<XRSessionSubsystem>();
                    if (subsystem != null)
                    {
                        subsystem.Start();
                    }
                }
            }
        }

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
            if (faceManager != null)
            {
                ConfigureFaceTracking(faceManager);
            }
        }
    }

    private void ConfigureARCamera(ARCameraManager cameraManager)
    {
        using (var configurations = cameraManager.GetConfigurations(Unity.Collections.Allocator.Temp))
        {
            if (configurations.Length > 0)
            {
                cameraManager.currentConfiguration = configurations[0];
            }
        }
    }

    private void ConfigureFaceTracking(ARFaceManager faceManager)
    {
        faceManager.enabled = true;
        faceManager.requestedMaximumFaceCount = 1;
    }
}