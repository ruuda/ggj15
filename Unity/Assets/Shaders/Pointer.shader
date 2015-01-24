Shader "Custom/Pointer" {
	Properties {
		color1 ("Color 1", Color) = (0.8, 0.223, 0.52, 0.851)
		color2 ("Color 2", Color) = (0.8, 0.223, 0.52, 0.851)
		speed ("Speed", float) = 2.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
		Blend SrcAlpha OneMinusSrcAlpha	
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			
			uniform float4 color1;
			uniform float4 color2;
			uniform float speed;
			
			struct vertIn
			{
			    float4 pos : POSITION;
			    float4 uv  : TEXCOORD0;
			};
			
			struct fragIn
			{
			    float4 pos   : SV_POSITION;
			};

			fragIn vert(vertIn i)
			{
			    fragIn o;
			    o.pos   = mul(UNITY_MATRIX_MVP, i.pos);
			    return o;
			}
			
			float4 mix(float4 x, float4 y, float m)
			{
			 	return x * (1.0 - m) + y * m;
			}

			
			float4 frag(fragIn i) : COLOR
			{  
				const float t = sin(_Time.x * speed) * 0.5 + 0.5;
				return mix(color1, color2, t);
			}
			
			ENDCG
		}
	} 
	FallBack "Unlit"
}
