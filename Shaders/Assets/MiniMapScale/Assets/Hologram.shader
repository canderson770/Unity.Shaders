Shader "Ben/Hologram"
{
    Properties
    {
		[Header(Wireframe Properties)]
		[Space(10)]
		_FrontColor ("Wireframe Color", color) = (1., 1., 1., 1.)
        _WireframeVal ("Wireframe width", Range(0., 0.5)) = 0.05
        
		
		[Header(Inner Model Properties)]
		[Space(10)]
		_TintColor("Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Range(0,1)) = .5
		
		[Header(Movement Properties)]
		[Space(10)]
		_Distance("Distance", Range (0,.05)) = 0
		_Amplitude("Amplitude", Range (0,100)) = 1
		_Speed("Speed", Range (0,25)) = 1
		_Amount("Amount", Range (0,1)) = 1
		//_EmissionLM ("Emission (Lightmapper)", Float) = 0
		//[Toggle] _DynamicEmissionLM ("Dynamic Emission (Lightmapper)", Int) = 0
        
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			ZWrite off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;
			float _Transparency;
			float _CutoutThresh;
			float _Distance;
			float _Amplitude;
			float _Speed;
			float _Amount;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
				col.a = _Transparency;
				clip(col.r - _CutoutThresh);
				return col;
			}
			ENDCG
		}
 
		//Blend One OneMinusDstAlpha
        Pass
        {
            Cull Back
			ZWrite off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
 
            
            #pragma shader_feature __ _REMOVEDIAG_ON
 
            #include "UnityCG.cginc"
 
            struct v2g {
                float4 worldPos : SV_POSITION;
            };

			float _Distance;
			float _Amplitude;
			float _Speed;
			float _Amount;
 
            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
            };
 
            v2g vert(appdata_base v) {
                v2g o;
				v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				//o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
                return o;
            }
 
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                float3 param = float3(0., 0., 0.);
 
                #if _REMOVEDIAG_ON
                float EdgeA = length(IN[0].worldPos - IN[1].worldPos);
                float EdgeB = length(IN[1].worldPos - IN[2].worldPos);
                float EdgeC = length(IN[2].worldPos - IN[0].worldPos);
               
                if(EdgeA > EdgeB && EdgeA > EdgeC)
                    param.y = 1.;
                else if (EdgeB > EdgeC && EdgeB > EdgeA)
                    param.x = 1.;
                else
                    param.z = 1.;
                #endif
 
                g2f o;
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float3(1., 0., 0.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float3(0., 0., 1.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float3(0., 1., 0.) + param;
                triStream.Append(o);
            }
 
            float _WireframeVal;
            fixed4 _FrontColor;
 
            fixed4 frag(g2f i) : SV_Target {
            if(!any(bool3(i.bary.x <= _WireframeVal, i.bary.y <= _WireframeVal, i.bary.z <= _WireframeVal)))
                 discard;
 
                return _FrontColor;
            }
 
            ENDCG
        }
    }
}