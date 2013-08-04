Shader "Custom/Projector" 
{
	Properties 
	{
		_MainTex ("Texture", 2D) = "white" { TexGen ObjectLinear }
		_Angle ("Culling Angle", Range(0,180)) = 5
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
		Offset -1, -1

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;	//	the position in the projectors world space
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			fixed _Angle;
			float4 _MainTex_ST;
			float4x4 _Projector;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = mul (_Projector, v.vertex);
				o.normal = mul (_Projector, half4(v.normal, 0));
				o.worldPos = mul (_Projector, v.vertex);
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 col = tex2Dproj (_MainTex, UNITY_PROJ_COORD(i.texcoord));
				half3 norm = (i.normal * .5) + .5;
				i.worldPos.y -= 5;
				fixed n = dot(i.worldPos, i.normal);

				if (i.texcoord.w < .5 || norm.b > .01)
					col.a = 0;

				return col * n;
			}
		ENDCG
		}
	} 
}