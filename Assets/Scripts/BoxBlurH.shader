Shader "Custom/BoxBlurH"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_Near ("Near", Float) = 0.2
		//_Range ("Range", Float) = 20.0
		_Spread("Spread", Float) = 5.0
		_BlurMult("BlurMult", Float) = 5.0
		//_HalfBlurH("HalfBlurH", Int) = 2
		//_HalfBlurV("HalfBlurV", Int) = 2
		_DivBlur("DivBlur", Float) = 1.0
		_Screen2Tex("Screen2Tex", Vector) = (1.0,1.0,0.0,0.0)

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
				float spread = _Spread * _Screen2Tex.x * _BlurMult;
				
				for(int x = -5; x <= 5; x++)
				{
					col += tex2D(_MainTex, i.uv + float2(x * spread, 0));
				}
				
				col *= _DivBlur;
				return col;
			}
			ENDCG
		}
	}
}
