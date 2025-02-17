using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class ARCameraConfigManager : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager cameraManager;
    
    private ARSession arSession;

    void Awake()
    {
        arSession = GetComponent<ARSession>();
        if (cameraManager == null)
            cameraManager = FindObjectOfType<ARCameraManager>();
    }

    void OnEnable()
    {
        if (cameraManager != null)
        {
            ConfigureCamera();
        }
    }

    private void ConfigureCamera()
    {
        using (var configurations = cameraManager.GetConfigurations(Allocator.Temp))
        {
            if (!configurations.IsCreated || configurations.Length == 0)
                return;

            int bestConfigIndex = 0;
            int maxResolution = 0;

            for (int i = 0; i < configurations.Length; i++)
            {
                var config = configurations[i];
                int resolution = config.resolution.x * config.resolution.y;
                
                if (resolution > maxResolution)
                {
                    maxResolution = resolution;
                    bestConfigIndex = i;
                }
            }

            cameraManager.currentConfiguration = configurations[bestConfigIndex];
        }
    }
}