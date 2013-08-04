Shader "Custom/Water Ripple" {
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Highlight ("Highlight", Color) = (1,1,1,1)
		_MainTex ("Normal (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
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

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.pos = mul(_Object2World, v.vertex);
				return o;
			}
			
			fixed4 _Color;
			fixed4 _Highlight;

			half4 frag (v2f i) : COLOR
			{
				half pixelHeight = tex2D( _MainTex, i.texcoord ).a;

				half acceptableHeightOffset = 0.001 * i.pos.x;
				half targetPixelHeight = fmod(_Time * 20, 1);

				fixed upper = pixelHeight - acceptableHeightOffset;

				return (pixelHeight > targetPixelHeight - acceptableHeightOffset && 
					upper < targetPixelHeight && upper < .9) ? 
						_Highlight : half4(0,1,0,0);
			}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
