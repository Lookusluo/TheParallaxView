using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

[RequireComponent(typeof(ARFace))]
public class ARFaceLandmarkManager : MonoBehaviour
{
    [SerializeField]
    private GameObject eyePositionMarker;
    
    private ARFace arFace;
    private ARKitFaceSubsystem arKitFaceSubsystem;
    private GameObject leftEyeMarker;
    private GameObject rightEyeMarker;

    private void Awake()
    {
        arFace = GetComponent<ARFace>();
        var faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null)
        {
            arKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
        }
    }

    private void OnEnable()
    {
        arFace.updated += OnFaceUpdated;
        SetupEyeMarkers();
    }

    private void OnDisable()
    {
        arFace.updated -= OnFaceUpdated;
    }

    private void SetupEyeMarkers()
    {
        if (eyePositionMarker != null)
        {
            leftEyeMarker = Instantiate(eyePositionMarker, transform);
            rightEyeMarker = Instantiate(eyePositionMarker, transform);
        }
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        if (arKitFaceSubsystem != null && arFace.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            UpdateEyePositions();
        }
    }

    private void UpdateEyePositions()
    {
        if (leftEyeMarker != null && rightEyeMarker != null)
        {
            leftEyeMarker.transform.position = arFace.leftEye.position;
            rightEyeMarker.transform.position = arFace.rightEye.position;
        }
    }
}