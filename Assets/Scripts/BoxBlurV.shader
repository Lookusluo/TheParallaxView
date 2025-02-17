Shader "Custom/BoxBlurV"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Spread ("Blur Spread", Float) = 3.0
    }
    
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Screen2Tex;
            float _Spread;
            float _BlurMult;
            float _DivBlur;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 0;
				float spread = _Spread * _Screen2Tex.y * _BlurMult;
				
				for(int y = -5; y <= 5; y++)
				{
					col += tex2D(_MainTex, i.uv + float2(0, y * spread));
				}
				
				col *= _DivBlur;
				return col;
			}
			ENDCG
		}
	}
}
