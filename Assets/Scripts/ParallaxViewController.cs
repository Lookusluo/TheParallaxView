using UnityEngine;

public class ParallaxViewController : MonoBehaviour
{
    [SerializeField]
    private float parallaxStrength = 1.0f;
    [SerializeField]
    private Vector2 parallaxLimit = new Vector2(0.5f, 0.5f);

    private Vector3 initialPosition;
    private Transform[] parallaxLayers;

    void Start()
    {
        initialPosition = transform.position;
        // 获取所有需要产生视差效果的层
        parallaxLayers = GetComponentsInChildren<Transform>();
    }

    public void UpdateViewerPosition(Vector3 eyePosition)
    {
        Vector3 viewerOffset = eyePosition - initialPosition;
        
        for (int i = 1; i < parallaxLayers.Length; i++)
        {
            float layerDepth = parallaxLayers[i].position.z - transform.position.z;
            Vector3 offset = new Vector3(
                Mathf.Clamp(viewerOffset.x * parallaxStrength * (layerDepth / 10f), -parallaxLimit.x, parallaxLimit.x),
                Mathf.Clamp(viewerOffset.y * parallaxStrength * (layerDepth / 10f), -parallaxLimit.y, parallaxLimit.y),
                0
            );
            
            parallaxLayers[i].localPosition = offset;
        }
    }
}