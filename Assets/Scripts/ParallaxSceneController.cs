using UnityEngine;

public class ParallaxSceneController : MonoBehaviour
{
    [SerializeField]
    private Transform viewerTransform;
    
    [SerializeField]
    private float parallaxDepthMultiplier = 1.0f;
    
    [SerializeField]
    private bool useSmoothing = true;
    
    [SerializeField]
    private float smoothingSpeed = 5.0f;

    private Vector3 lastViewerPosition;
    private Vector3 targetPosition;
    private Transform[] parallaxLayers;

    private void Start()
    {
        parallaxLayers = GetComponentsInChildren<Transform>();
        if (viewerTransform != null)
        {
            lastViewerPosition = viewerTransform.position;
        }
    }

    private void LateUpdate()
    {
        if (viewerTransform == null) return;

        Vector3 viewerDelta = viewerTransform.position - lastViewerPosition;
        
        for (int i = 1; i < parallaxLayers.Length; i++)
        {
            float layerDepth = parallaxLayers[i].position.z;
            float parallaxFactor = (layerDepth * parallaxDepthMultiplier);
            
            targetPosition = parallaxLayers[i].position + new Vector3(
                viewerDelta.x * parallaxFactor,
                viewerDelta.y * parallaxFactor,
                0
            );

            if (useSmoothing)
            {
                parallaxLayers[i].position = Vector3.Lerp(
                    parallaxLayers[i].position,
                    targetPosition,
                    Time.deltaTime * smoothingSpeed
                );
            }
            else
            {
                parallaxLayers[i].position = targetPosition;
            }
        }

        lastViewerPosition = viewerTransform.position;
    }
}