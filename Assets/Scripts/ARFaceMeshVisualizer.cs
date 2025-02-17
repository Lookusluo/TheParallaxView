using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class ARFaceMeshVisualizer : MonoBehaviour
{
    [SerializeField]
    private bool showMesh = false;
    
    private ARFace arFace;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        arFace = GetComponent<ARFace>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        arFace.updated += OnFaceUpdated;
        UpdateVisibility();
    }

    private void OnDisable()
    {
        arFace.updated -= OnFaceUpdated;
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = showMesh && arFace.trackingState == TrackingState.Tracking;
        }
    }

    public void ToggleMeshVisibility()
    {
        showMesh = !showMesh;
        UpdateVisibility();
    }
}