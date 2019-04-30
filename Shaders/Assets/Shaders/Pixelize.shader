﻿// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Cody/Other/Pixelize" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Pixels ("Pixels", Vector) = (10,10,0,0)

//		_LCDTex("LCD (RGB)", 2D) = "white" {}
//		_LCDPixels("LCD pixels", Vector) = (3,3,0,0)
//
//		_DistanceOne ("Distance of full effect", Float) = 0.5 // In metres
//		_DistanceZero ("Distance of zero effect", Float) = 1 // In metres

		_Brightness("Brightness",  Range(0,5)) = 3
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _LCDTex;
		
		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		float4 _Pixels, _LCDPixels;
		float _Brightness, _DistanceOne, _DistanceZero;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			float2 uv = round(IN.uv_MainTex * _Pixels.xy + 0.5) / _Pixels.xy;
			fixed4 a = tex2D(_MainTex, uv);
			//fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

//			float2 uv_lcd = IN.uv_MainTex * _Pixels.xy / _LCDPixels;
//			fixed4 d = tex2D(_LCDTex, uv_lcd);
//
//			float dist = distance(_WorldSpaceCameraPos, IN.worldPos);
//			float alpha = saturate((dist - _DistanceOne) / (_DistanceZero-_DistanceOne));	
			// [_DistanceOne, _DistanceZero] > [0, 1]

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
//
//			float2 scaledUV = IN.uv_MainTex * _Pixels.xy;
//			float2 pixelDeriv = fwidth(scaledUV);
//			float pixelWidth = length(pixelDeriv);
			 
			o.Albedo = a  * _Brightness;
//			o.Albedo = lerp(a * d, a, alpha) * _Brightness;		
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
