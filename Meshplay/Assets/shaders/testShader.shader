Shader "Blush" {
Properties {
	_Color ("Main Color", Color) = (0,0,0,1)
	_MainTex ("Base (RGB)", 2D) = "black" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

Cull Off

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = 1.0;
}
ENDCG
}

Fallback "VertexLit"
}