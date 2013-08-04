Shader "Custom/Water" {
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Highlight ("Highlight", Color) = (1,1,1,1)
		_MainTex ("Normal (RGB)", 2D) = "white" {}
		_WaveHeight ("Wave Height", Range(0, .2)) = .1 
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 pos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _WaveHeight;
			
			v2f vert (appdata_base v)
			{
				v2f o;

				half4 worldPos = mul(_Object2World, v.vertex);

				v.vertex.y += sin(_Time * worldPos.z) * _WaveHeight;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.pos = worldPos;
				return o;
			}
			
			fixed4 _Color;
			fixed4 _Highlight;

			half4 frag (v2f i) : COLOR
			{
				half pixelHeight = tex2D( _MainTex, i.texcoord ).a;

				half acceptableHeightOffset = 0.02;
				half targetPixelHeight = abs(sin(_Time * 10 - (i.pos.x - i.pos.y) * .5) * 2) * .1;

				return (pixelHeight > targetPixelHeight - acceptableHeightOffset && 
					pixelHeight < targetPixelHeight + acceptableHeightOffset) ? 
						_Highlight : _Color;
			}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
