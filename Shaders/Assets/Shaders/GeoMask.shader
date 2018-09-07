Shader "Cody/GeoMask"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_ZTest("ZTest", Range(0,12)) = 0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Overlay+1" }
		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha

		ZTest Greater
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _Transparency;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
//		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
//		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
//			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
	    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	    o.Albedo = c.rgb;
	    o.Alpha = c.a;
//	    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
		ENDCG
	}


	FallBack "Diffuse"
}
