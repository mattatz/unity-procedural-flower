Shader "mattatz/ProceduralFlower/Stover"
{
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Color2 ("Color (2nd)", Color) = (1, 1, 1, 1)
		_Bend ("Bend", Range(0.0, 1.0)) = 0.0
		_T ("T", Range(0.0, 1.0)) = 1.0
		_Intensity ("Lighting intensity", Range(0.0, 1.0)) = 1.0
	}

	CGINCLUDE

	#pragma multi_compile_fwdbase
	#include "UnityCG.cginc"
	#include "AutoLight.cginc"

	ENDCG

	SubShader {

		Pass {
			Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
			Lighting On ZWrite On Cull Back
			LOD 100
			CGPROGRAM
			#define CULL_BACK 1
			#include "ProceduralFlower.cginc"
			#pragma vertex vert_common
			#pragma fragment frag_common
			ENDCG
		}

		Pass {
			Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
			Lighting On ZWrite On Cull Front
			LOD 100
			CGPROGRAM
			#define CULL_FRONT 1
			#include "ProceduralFlower.cginc"
			#pragma vertex vert_common
			#pragma fragment frag_common
			ENDCG
		}

		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			ZWrite On ZTest LEqual Cull Off

			CGPROGRAM
			#include "ProceduralFlower.cginc"
			#pragma multi_compile_shadowcaster
			#pragma vertex vert_shadow_common
			#pragma fragment frag_shadow_common
			ENDCG
		}
	}
}
