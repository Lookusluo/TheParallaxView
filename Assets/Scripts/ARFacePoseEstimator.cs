using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFacePoseEstimator : MonoBehaviour
{
    [SerializeField]
    private ARFaceManager faceManager;
    [SerializeField]
    private float smoothingFactor = 0.1f;
    
    private ARFace targetFace;
    private Vector3 smoothedPosition;
    private Quaternion smoothedRotation;

    private void Start()
    {
        if (faceManager == null)
        {
            faceManager = FindObjectOfType<ARFaceManager>();
        }
        
        if (faceManager != null)
        {
            faceManager.facesChanged += OnTrackablesChanged;
        }
    }

    private void OnDestroy()
    {
        if (faceManager != null)
        {
            faceManager.facesChanged -= OnTrackablesChanged;
        }
    }

    private void OnTrackablesChanged(ARFacesChangedEventArgs args)
    {
        foreach (var addedFace in args.added)
        {
            targetFace = addedFace;
            break;
        }

        foreach (var removedFace in args.removed)
        {
            if (targetFace != null && removedFace.trackableId == targetFace.trackableId)
            {
                targetFace = null;
            }
        }
    }

    private void Update()
    {
        if (targetFace != null && targetFace.trackingState == TrackingState.Tracking)
        {
            smoothedPosition = Vector3.Lerp(smoothedPosition, targetFace.transform.position, smoothingFactor);
            smoothedRotation = Quaternion.Lerp(smoothedRotation, targetFace.transform.rotation, smoothingFactor);
            
            transform.position = smoothedPosition;
            transform.rotation = smoothedRotation;
        }
    }

    public Vector3 GetEstimatedPosition()
    {
        return smoothedPosition;
    }

    public Quaternion GetEstimatedRotation()
    {
        return smoothedRotation;
    }
}