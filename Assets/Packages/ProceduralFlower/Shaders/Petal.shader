Shader "ProceduralFlower/Petal" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Bend ("Bend", Float) = 0.5
		_Cull ("Cull", Int) = 0.0 // Off
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass {
			Cull [_Cull]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			#define HALF_PI 1.570795
			#define PI 3.14159

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed _Bend;
			
			v2f vert (appdata v) {
				v2f o;

				float bend = pow(abs(v.vertex.x), 2.0) * _Bend;
				v.vertex.z += bend;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.uv);
				col = fixed4((normalize(i.normal) + 1.0) * 0.5, 1.0);
				// col = fixed4(abs(i.uv.x), i.uv.y, 0, 1.0);
				return col;
			}

			ENDCG
		}
	}
}
