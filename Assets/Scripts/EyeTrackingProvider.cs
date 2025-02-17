using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EyeTrackingProvider : MonoBehaviour
{
    [SerializeField]
    private ARFace targetFace;
    
    private Vector3 leftEyePosition;
    private Vector3 rightEyePosition;
    private bool isTracking;

    public bool IsTracking => isTracking;
    public Vector3 LeftEyePosition => leftEyePosition;
    public Vector3 RightEyePosition => rightEyePosition;
    public float InterPupillaryDistance => Vector3.Distance(leftEyePosition, rightEyePosition);

    private void OnEnable()
    {
        if (targetFace != null)
        {
            targetFace.updated += OnFaceUpdated;
        }
    }

    private void OnDisable()
    {
        if (targetFace != null)
        {
            targetFace.updated -= OnFaceUpdated;
        }
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        if (targetFace.trackingState == TrackingState.Tracking)
        {
            UpdateEyePositions();
            isTracking = true;
        }
        else
        {
            isTracking = false;
        }
    }

    private void UpdateEyePositions()
    {
        leftEyePosition = targetFace.leftEye.position;
        rightEyePosition = targetFace.rightEye.position;
    }
}