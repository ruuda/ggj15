Shader "Custom/UnlitFog" {
	Properties {
		base ("Base (RGB)", 2D) = "white" {}
		fogColourLow ("Fog Colour Low", Color) = (0.8, 0.223, 0.52, 0.851)
		fogColourHigh ("Fog Colour High", Color) = (1.0, 0.588, 0.557, 0.851)
		fogDistance ("Fog Distance", Float) = 5.0
		fogHeight ("Fog Height", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {		
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityShaderVariables.cginc"
			
			uniform sampler2D base;
			uniform float4 fogColourLow;
			uniform float4 fogColourHigh;
			uniform float fogDistance;
			uniform float fogHeight;
			
			struct vertIn
			{
			    float4 pos : POSITION;
			    float4 uv  : TEXCOORD0;
			};
			
			struct fragIn
			{
			    float4 pos   : SV_POSITION;
			    float  depth : COLOR0;
			    float4 uv    : TEXCOORD0;
			    float4 wpos  : TEXCOORD1;
			};

			fragIn vert(vertIn i)
			{
			    fragIn o;
			    o.pos   = mul(UNITY_MATRIX_MVP, i.pos);
			    o.wpos  = mul(_Object2World, i.pos);
			    o.uv    = i.uv; 
			    o.depth = o.pos.z / fogDistance;
			    return o;
			}
			
			float4 mix(float4 x, float4 y, float m)
			{
			 	return x * (1.0 - m) + y * m;
			}

			
			float4 frag(fragIn i) : COLOR
			{  
				const float4 channels = tex2D(base, i.uv.xy);
				const float4 f = mix(fogColourLow, fogColourHigh, 1.0 - exp(-i.wpos.y / fogHeight));
				const float4 withFog = mix(channels, f, (1.0 - exp(-i.depth)) * f.a);
				return float4(withFog.rgb, 1.0f);
			}
			
			ENDCG
		}
	} 
	FallBack "Unlit"
}
