using UnityEngine;
using UnityEngine.XR.Management;

public class XRPackageInitializer : MonoBehaviour
{
    void Awake()
    {
        var generalSettings = XRGeneralSettings.Instance;
        if (generalSettings == null)
        {
            Debug.LogError("XR General Settings not found");
            return;
        }

        var manager = generalSettings.Manager;
        if (!manager.isInitializationComplete)
        {
            manager.InitializeLoaderSync();
        }
    }

    void OnDestroy()
    {
        var generalSettings = XRGeneralSettings.Instance;
        if (generalSettings != null && generalSettings.Manager != null)
        {
            generalSettings.Manager.DeinitializeLoader();
        }
    }
}