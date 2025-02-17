using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SelectEye : MonoBehaviour 
{
    public ARFaceTrackingManager headTrackManager;
    public CameraManager camManager;
    public Transform leftEye;
    public Transform rightEye;
    public Transform thirdEye;

    [SerializeField]
    private float smoothingFactor = 0.1f;
    private Vector3 targetPosition;

    void Update()
    {
        Vector3 pos = transform.localPosition;
        float IPD = headTrackManager.IPD;
        float EyeHeight = headTrackManager.EyeHeight;

        float dist = IPD * 0.001f * 0.5f;
        float height = EyeHeight * 0.001f;

        // Calculate target position based on selected eye
        targetPosition = pos;
        if (headTrackManager.openEye == ARFaceTrackingManager.OpenEye.Right)
        {
            targetPosition.x = camManager.DeviceCamUsed ? dist : -dist;
        }
        else
        {
            targetPosition.x = camManager.DeviceCamUsed ? -dist : dist;
        }

        // Apply smoothing to camera movement
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smoothingFactor * Time.deltaTime);

        // Update third eye height with smoothing
        Vector3 thirdEyePos = thirdEye.transform.localPosition;
        thirdEyePos.y = Mathf.Lerp(thirdEyePos.y, height, smoothingFactor * Time.deltaTime);
        thirdEye.transform.localPosition = thirdEyePos;

        // Update visualization markers
        rightEye.localPosition = new Vector3(dist, 0, 0);
        leftEye.localPosition = new Vector3(-dist, 0, 0);
    }
}