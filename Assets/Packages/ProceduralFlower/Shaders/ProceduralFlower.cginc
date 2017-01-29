#ifndef _PROCEDURAL_FLOWER_COMMON_
#define _PROCEDURAL_FLOWER_COMMON_

#define PF_HALF_PI 1.570795
#define PF_PI 3.14159

fixed4 _Color;
float _Bend, _T;

uniform float4 _LightColor0;

float calculate_bend_amount (float3 v) {
	return pow(abs(v.x), 2.0) * _Bend;
}

float3 bend_petal (float3 v) {
	float bend = calculate_bend_amount(v);
	v.z += bend;
	return v;
}

float3 bend_bud (float3 v) {
	float bend = calculate_bend_amount(v);
	v.z += bend - (sin(v.y * PF_PI) * 0.15 * _Bend);
	return v;
}

void fade (float2 uv) {
	clip(_T - uv.y);
}

struct appdata {
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
};

struct v2f {
	float4 pos : SV_POSITION;
	float3 lightDir : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float2 uv : TEXCOORD2;
	LIGHTING_COORDS(3, 4)
};

struct v2f_shadow {
	V2F_SHADOW_CASTER;
	float2 uv : TEXCOORD0;
};

v2f vert_common (appdata v) {
	v2f OUT;
	OUT.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	OUT.uv = v.uv;
	OUT.lightDir = normalize(ObjSpaceLightDir(v.vertex));
	OUT.normal = v.normal.xyz;

	TRANSFER_VERTEX_TO_FRAGMENT(OUT);
	TRANSFER_SHADOW(OUT);
	return OUT;
}

v2f_shadow vert_shadow_common (appdata v) {
	v2f_shadow OUT;
	TRANSFER_SHADOW_CASTER_NORMALOFFSET(OUT)
	OUT.uv = v.uv;
	return OUT;
}

v2f vert_petal (appdata v) {
	v.vertex.xyz = bend_petal(v.vertex);
	return vert_common(v);
}

v2f_shadow vert_shadow_petal (appdata v) {
	v.vertex.xyz = bend_petal(v.vertex);
	return vert_shadow_common(v);
}

v2f vert_bud (appdata v) {
	v.vertex.xyz = bend_bud(v.vertex);
	return vert_common(v);
}

v2f_shadow vert_shadow_bud (appdata v) {
	v.vertex.xyz = bend_bud(v.vertex);
	return vert_shadow_common(v);
}

float4 frag_common (v2f IN) : SV_Target {
	fade(IN.uv);

	fixed atten = LIGHT_ATTENUATION(IN);
	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT;
	fixed d = saturate(dot(normalize(IN.lightDir), normalize(IN.normal)));
	fixed4 c = _Color;
	c.rgb = (c * _LightColor0 * atten * d) + ambient;
	return c;
}

float4 frag_shadow_common (v2f_shadow IN) : COLOR {
	fade(IN.uv);

	SHADOW_CASTER_FRAGMENT(IN)
}

#endif
