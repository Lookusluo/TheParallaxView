using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "TheParallaxView/SceneInfo")]
public class SceneInfo : MonoBehaviour
{
    public string sceneName;
    public bool use;
    public Color ambientLight = Color.white;
    public Color bgColor = Color.black;
    public bool headLight = true;
    
    [Header("Post Processing")]
    public bool usePostProcessing;
    public float blurIntensity = 1f;
    
    [Header("Scene Settings")]
    public float parallaxStrength = 1f;
    public bool useDepthOfField;
}