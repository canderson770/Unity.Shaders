// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Cody/LoadingScreen/3DPrinting" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ConstructY("Construct Y", Range(-1,10)) = 0
		_ConstructColor("Construct Color", Color) = (1,1,1,1)
		_ConstructGap("_ConstructGap", Range(0,.1)) = 0
		_ConstructWaveAmount("_ConstructWaveAmount", Range(0,2)) = 1
		_ConstructWaveIntensity("_ConstructWaveIntensity", Range(100,500)) = 100
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Cull Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _ConstructY, _ConstructGap, _ConstructWaveAmount, _ConstructWaveIntensity;
		fixed4 _ConstructColor;
		int building;
		float3 viewDir;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			viewDir = IN.viewDir;

			float s = +sin((IN.worldPos.x * IN.worldPos.z) * _ConstructWaveAmount + _Time[2] + o.Normal) / _ConstructWaveIntensity;

			if (IN.worldPos.y > _ConstructY + s + _ConstructGap)
				discard;

			if (IN.worldPos.y < _ConstructY)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha  = c.a;

				building = 0;
			}
			else
			{
				o.Albedo = _ConstructColor.rgb;
				o.Alpha  = _ConstructColor.a;

				building = 1;
			}

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}

//		#pragma surface surf Unlit fullforwardshadows
//		inline half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten)
//		{
//			return _ConstructColor;
//		}
//
//		inline half4 LightingUnlit_GI(SurfaceOutput s, half3 lightDir, UnityGI gi)
//		{
//			return _ConstructColor;
//		}
//
//		inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
//		{
//			if (!building)
//				return LightingStandard(s, lightDir, gi); // Unity5 PBR
//
//			if (dot(s.Normal, viewDir) < 0)
//				return _ConstructColor;
//
//			return _ConstructColor; // Unlit
//		}
//
//		inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
//		{
//			LightingStandard_GI(s, data, gi);		
//		}
		ENDCG
	}
	FallBack "Diffuse"
}
