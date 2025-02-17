using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;

public class ARFaceTrackingManager : MonoBehaviour
{
    [SerializeField]
    private ARFaceManager faceManager;
    [SerializeField]
    private GameObject parallaxViewPrefab;
    [SerializeField]
    private float smoothingFactor = 0.3f;
    
    private ARFace currentTrackedFace;
    private GameObject parallaxInstance;
    private Vector3 smoothedLeftEyePos;
    private Vector3 smoothedRightEyePos;
    
    public float IPD { get; private set; }
    public float EyeHeight { get; private set; }
    public bool IsTracking => currentTrackedFace?.trackingState == TrackingState.Tracking;
    public enum OpenEye { Left, Right }
    public OpenEye openEye = OpenEye.Left;

    void Start()
    {
        if (faceManager == null)
        {
            var xrOrigin = FindObjectOfType<XROrigin>();
            if (xrOrigin != null)
            {
                faceManager = xrOrigin.GetComponent<ARFaceManager>();
            }
        }

        if (faceManager != null)
        {
            faceManager.facesChanged += OnFacesChanged;
            faceManager.enabled = true;
        }
    }

    void OnDestroy()
    {
        if (faceManager != null)
        {
            faceManager.facesChanged -= OnFacesChanged;
        }
    }

    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        foreach (var addedFace in args.added)
        {
            if (currentTrackedFace == null)
            {
                currentTrackedFace = addedFace;
                currentTrackedFace.updated += OnFaceUpdated;
                
                if (parallaxInstance == null)
                {
                    parallaxInstance = Instantiate(parallaxViewPrefab);
                }
            }
        }

        foreach (var removedFace in args.removed)
        {
            if (currentTrackedFace != null && removedFace.trackableId == currentTrackedFace.trackableId)
            {
                if (currentTrackedFace != null)
                {
                    currentTrackedFace.updated -= OnFaceUpdated;
                }
                currentTrackedFace = null;
                
                if (parallaxInstance != null)
                {
                    Destroy(parallaxInstance);
                }
            }
        }
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        if (currentTrackedFace.trackingState == TrackingState.Tracking)
        {
            UpdateEyeTracking();
        }
    }

    private void UpdateEyeTracking()
    {
        if (currentTrackedFace.leftEye != null && currentTrackedFace.rightEye != null)
        {
            Vector3 leftEyePos = currentTrackedFace.leftEye.position;
            Vector3 rightEyePos = currentTrackedFace.rightEye.position;

            // Apply smoothing to eye positions
            smoothedLeftEyePos = Vector3.Lerp(smoothedLeftEyePos, leftEyePos, smoothingFactor);
            smoothedRightEyePos = Vector3.Lerp(smoothedRightEyePos, rightEyePos, smoothingFactor);
            
            IPD = Vector3.Distance(smoothedLeftEyePos, smoothedRightEyePos) * 1000f;
            EyeHeight = (smoothedLeftEyePos.y + smoothedRightEyePos.y) * 0.5f;
        }
    }

    public Vector3 GetCurrentEyePosition()
    {
        return openEye == OpenEye.Left ? smoothedLeftEyePos : smoothedRightEyePos;
    }
}