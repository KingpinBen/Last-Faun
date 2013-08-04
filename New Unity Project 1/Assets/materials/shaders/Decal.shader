Shader "Custom/Decal" {
   Properties {
      _MainTex ("Cookie", 2D) = "gray" { TexGen ObjectLinear }
      _FalloffTex ("FallOff", 2D) = "white" { TexGen ObjectLinear   }
   }

   Subshader {
      Tags {  "Queue"="Transparent" 
			  "IgnoreProjector"="True" 
			  "RenderType"="Transparent" }
      Pass {
         ZWrite Off
         AlphaTest Greater 0
		 LOD 200
		 Cull back
         ColorMask RGB
         Blend DstColor Zero
		 Offset -1, -1

         SetTexture [_MainTex] {
            combine texture, ONE - texture
            Matrix [_Projector]
         }

         SetTexture [_FalloffTex] {
            constantColor (1,1,1,0)
            combine previous lerp (texture) constant
            Matrix [_ProjectorClip]
         }
      }
   }
}