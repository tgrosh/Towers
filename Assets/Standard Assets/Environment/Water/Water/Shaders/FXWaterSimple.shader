// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "FX/WaterSimple" {
Properties {
	_WaveScale ("Wave scale", Range (0.02,0.15)) = 0.063
	_ReflDistort ("Reflection distort", Range (0,1.5)) = 0.44
	_RefrDistort ("Refraction distort", Range (0,1.5)) = 0.40
	_RefrColor ("Refraction color", COLOR)  = ( .34, .85, .92, 1)
	_DepthColor("Color", Color) = (1,1,1,1)
	_DepthFade("Depth Fade", Float) = 1.0
	_DepthDistance("Depth Distance", Float) = -0.09
	[NoScaleOffset] _Fresnel ("Fresnel (A) ", 2D) = "gray" {}
	[NoScaleOffset] _Foam ("Foam texture", 2D) = "white" {}
	[NoScaleOffset] _FoamGradient ("Foam gradient ", 2D) = "white" {}
	_FoamStrength ("Foam strength", Range (0, 10.0)) = 1.0
	WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
	[NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
	_HorizonColor ("Simple water horizon color", COLOR)  = ( .172, .463, .435, 1)
	[HideInInspector] _ReflectionTex ("Internal Reflection", 2D) = "" {}
	[HideInInspector] _RefractionTex ("Internal Refraction", 2D) = "" {}
}


// -----------------------------------------------------------
// Fragment program cards


Subshader {
	Tags { "WaterMode"="Refractive" "RenderType"="Opaque" }
	Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#pragma multi_compile WATER_REFRACTIVE WATER_REFLECTIVE WATER_SIMPLE

#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
#define HAS_REFLECTION 1
#endif
#if defined (WATER_REFRACTIVE)
#define HAS_REFRACTION 1
#endif

#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
#define HAS_FOAM
#endif

#include "UnityCG.cginc"

uniform float4 _WaveScale4;
uniform float4 _WaveOffset;
half _DepthFade;
half _DepthDistance;
fixed4 _DepthColor;

#if defined(HAS_FOAM)
uniform float _FoamStrength;
uniform sampler2D _CameraDepthTexture; //Depth Texture
#endif

#if HAS_REFLECTION
uniform float _ReflDistort;
#endif
#if HAS_REFRACTION
uniform float _RefrDistort;
#endif

struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};

struct v2f {
	float4 pos : SV_POSITION;
	#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
		float4 ref : TEXCOORD0;
		float3 viewDir : TEXCOORD3;
		#if defined(HAS_FOAM)
			float2 foamuv : TEXCOORD4;
		#endif
	#else
		float3 viewDir : TEXCOORD2;
	#endif
	
	UNITY_FOG_COORDS(4)
};

v2f vert(appdata v)
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	
	// scroll foam
	float4 wpos = mul (unity_ObjectToWorld, v.vertex);
	
	#if defined(HAS_FOAM)	
	o.foamuv = 7.0f * wpos.xz + 0.05 * float2(_SinTime.w, _SinTime.w);
	#endif
	
	// object space view direction (will normalize per pixel)
	o.viewDir.xzy = WorldSpaceViewDir(v.vertex);
	
	#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
	o.ref = ComputeScreenPos(o.pos);
	#endif

	UNITY_TRANSFER_FOG(o,o.pos);
	return o;
}

#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
sampler2D _ReflectionTex;
#endif
#if defined (WATER_REFLECTIVE) || defined (WATER_SIMPLE)
sampler2D _ReflectiveColor;
#endif
#if defined (WATER_REFRACTIVE)
sampler2D _Fresnel;
sampler2D _RefractionTex;
uniform float4 _RefrColor;
#endif
#if defined (WATER_SIMPLE)
uniform float4 _HorizonColor;
#endif

#if defined(HAS_FOAM)
sampler2D _Foam;
sampler2D _FoamGradient;
#endif

half4 frag( v2f i ) : SV_Target
{
	i.viewDir = normalize(i.viewDir);
		
	// fresnel factor
	half fresnelFac = i.viewDir;
	
	// perturb reflection/refraction UVs, and lookup colors	
	#if HAS_REFLECTION
	float4 uv1 = i.ref;
	half4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1) );
	#endif
	#if HAS_REFRACTION
	float4 uv2 = i.ref;
	half4 refr = tex2Dproj( _RefractionTex, UNITY_PROJ_COORD(uv2) ) * _RefrColor;
	#endif
	
	// final color is between refracted and reflected based on fresnel
	half4 color;
	
	#if defined(WATER_REFRACTIVE)
	half fresnel = UNITY_SAMPLE_1CHANNEL( _Fresnel, float2(fresnelFac,fresnelFac) );
	color = lerp( refr, refl, fresnel);
	#endif
	
	#if defined(WATER_REFLECTIVE)
	half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
	color.rgb = lerp( water.rgb, refl.rgb, water.a );
	color.a = refl.a * water.a;
	#endif
	
	#if defined(WATER_SIMPLE)
	half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
	color.rgb = lerp( water.rgb, _HorizonColor.rgb, water.a );
	color.a = _HorizonColor.a;
	#endif
	
	#if defined(HAS_FOAM)
	float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.ref)).r);
	float objectZ = i.ref.w;
	float intensityFactor = 1 - saturate((sceneZ - objectZ) / _FoamStrength);    
	half3 foamGradient = 1 - tex2D(_FoamGradient, float2(intensityFactor - _Time.y*0.15, 0) );
	float2 foamDistortUV = 0.2;
	half3 foamColor = tex2D(_Foam, i.foamuv + foamDistortUV).rgb;
	half foamLightIntensity = saturate((-_WorldSpaceLightPos0.y + 0.2) * 4);
	color.rgb += foamGradient * intensityFactor * foamColor * foamLightIntensity;
	#endif
	
	UNITY_APPLY_FOG(i.fogCoord, color);
	half depth = sceneZ - i.ref.w - _DepthDistance;
	return lerp(color, _DepthColor, saturate(depth * _DepthFade));
}
ENDCG

	}
}

}
