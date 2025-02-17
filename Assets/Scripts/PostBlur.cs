using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBlur : MonoBehaviour {

	//public Material mat;


	public Material matH;
	public Material matV;

	public Camera cam;
	public OffAxisProjection proj;
	bool isActive = false;

	[Space(20)]

	public float pass1_BlurMult = 10f;
	public float pass1_Spread = 5f;

	[Space(20)]

	public float pass2_BlurMult = 7f;
	public float pass2_Spread = 4f;

	[Space(20)]

	public float pass3_BlurMult = 4f;
	public float pass3_Spread = 3f;

	[Space(20)]

	public float pass4_BlurMult = 3f;
	public float pass4_Spread = 1f;


	// Use this for initialization
	void Start () {
		//cam.depthTextureMode = DepthTextureMode.Depth;
	}
	
	// Update is called once per frame
	void Update () {
		//mat.SetFloat ("_Near", proj.nearDist);
	}


	public void Activate() {
		//cam.depthTextureMode = DepthTextureMode.Depth;
		isActive = true;
	}

	public void DeActivate() {
		//cam.depthTextureMode = DepthTextureMode.None;
		isActive = false;
	}


	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
	    if (!isActive || matH == null || matV == null)
	    {
	        Graphics.Blit(src, dst);
	        return;
	    }
	
	    Vector2 texelSize = new Vector2(1f / Screen.width, 1f / Screen.height);
	    matH.SetVector("_Screen2Tex", texelSize);
	    matV.SetVector("_Screen2Tex", texelSize);
	    
	    float divBlur = 1f / 11f;
	    matH.SetFloat("_DivBlur", divBlur);
	    matV.SetFloat("_DivBlur", divBlur);
	
	    RenderTexture temp = RenderTexture.GetTemporary(Screen.width, Screen.height);
	    
	    // 应用多遍模糊
	    ApplyBlurPass(src, temp, dst);
	
	    RenderTexture.ReleaseTemporary(temp);
	}

	private void ApplyBlurPass(RenderTexture source, RenderTexture temp, RenderTexture destination)
	{
	    // Pass 1
	    matH.SetFloat("_Spread", pass1_Spread);
	    matH.SetFloat("_BlurMult", pass1_BlurMult);
	    Graphics.Blit(source, temp, matH);
	
	    // Pass 2
	    matV.SetFloat("_Spread", pass2_Spread);
	    matV.SetFloat("_BlurMult", pass2_BlurMult);
	    Graphics.Blit(temp, source, matV);
	
	    // Pass 3
	    matH.SetFloat("_Spread", pass3_Spread);
	    matH.SetFloat("_BlurMult", pass3_BlurMult);
	    Graphics.Blit(source, temp, matH);
	
	    // Pass 4
	    matV.SetFloat("_Spread", pass4_Spread);
	    matV.SetFloat("_BlurMult", pass4_BlurMult);
	    Graphics.Blit(temp, destination, matV);
	}
}
