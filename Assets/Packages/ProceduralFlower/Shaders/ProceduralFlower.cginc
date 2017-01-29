#ifndef _PROCEDURAL_FLOWER_COMMON_
#define _PROCEDURAL_FLOWER_COMMON_

#define PF_HALF_PI 1.570795
#define PF_PI 3.14159

float _Bend;

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

void pf_vert_petal (inout appdata_full v) {
	v.vertex.xyz = bend_petal(v.vertex.xyz);
}

void pf_vert_bud (inout appdata_full v) {
	v.vertex.xyz = bend_bud(v.vertex.xyz);
}

#endif
