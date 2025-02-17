using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadTrackManager : MonoBehaviour
{
    public float IPD = 63.0f;
    public float EyeHeight = 0.0f;
    public string eyeInfoText;
    public string ARError;

    [SerializeField]
    private ARFaceManager faceManager;
    private ARFace currentFace;

    public enum OpenEye { Left, Right }
    public OpenEye openEye = OpenEye.Left;

    void Start()
    {
        faceManager = GetComponent<ARFaceManager>();
        if (faceManager != null)
        {
            faceManager.facesChanged += OnFacesChanged;
        }
        else
        {
            ARError = "AR Face Manager not found";
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
            currentFace = addedFace;
            ARError = null;
            break;
        }

        foreach (var removedFace in args.removed)
        {
            if (currentFace != null && removedFace.trackableId == currentFace.trackableId)
            {
                currentFace = null;
            }
        }
    }

    void Update()
    {
        if (currentFace != null && currentFace.trackingState == TrackingState.Tracking)
        {
            UpdateEyeTracking();
        }
    }

    private void UpdateEyeTracking()
    {
        if (currentFace.leftEye != null && currentFace.rightEye != null)
        {
            Vector3 leftEyePos = currentFace.leftEye.position;
            Vector3 rightEyePos = currentFace.rightEye.position;
            
            IPD = Vector3.Distance(leftEyePos, rightEyePos) * 1000f; // 转换为毫米
            EyeHeight = (leftEyePos.y + rightEyePos.y) * 0.5f;
            
            eyeInfoText = $"Eye tracking active\nLeft eye: {leftEyePos}\nRight eye: {rightEyePos}";
        }
    }
}