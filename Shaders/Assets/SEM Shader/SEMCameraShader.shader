Shader "Cody/SEMScreenShader"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Render Texture", 2D) = "white" {}
		[Toggle] _GrayScaleOn("Grayscale", Range(0, 1)) = 1
		_Color("Tint", Color) = (0.0, 0.7, 1.0, 0.2)

		[Space(20)]
		[NoScaleOffset] _NoiseTex("Noise Texture", 2D) = "white" {}
		[Toggle] _Speed("Move", Range(0,1)) = 1

		[Space(20)]
		_TopClarity("Top Clarity", Range(1,15)) = 4
		_BottomClarity("Bottom Clarity", Range (1,15)) = 2

		[Space(20)]
		_ScanPos("Scan line position", float) = 0
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3: TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};
					
			uniform half _OffsetX, _OffsetY;
			uniform fixed _Speed;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord + float2(_OffsetX, _OffsetY);
				o.uv2 = v.texcoord + float2(_Time.z * _Speed, _Time.z * _Speed);
				o.uv3 = v.texcoord + float2(_Time.z * _Speed, _Time.z * _Speed);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			uniform fixed4 _Color;
			uniform sampler2D _MainTex, _NoiseTex;
			uniform float _GrayScaleOn, _ScanPos;
			uniform float _BottomClarity, _TopClarity;

			fixed4 frag(v2f i) : SV_Target
			{
				//calculate clarity of top
				fixed noiseScaleTop = _TopClarity;
				fixed noiseIntensityTop = 1 / (_TopClarity + .3);

				//calculate clarity of bottom
				fixed noiseScaleBottom = _BottomClarity;
				fixed noiseIntensityBottom = 1 / (_BottomClarity + .3);
						
				//main texture
				fixed4 col = tex2D(_MainTex, i.uv);

				//noise textures
				fixed4 noiseTop = tex2D(_NoiseTex, i.uv3 * noiseScaleTop) * noiseIntensityTop;
				fixed4 noiseBottom = tex2D(_NoiseTex, i.uv2 * noiseScaleBottom) * noiseIntensityBottom;
							
				//make black and white, if grayscale is on
				col.rgb = (((col.r + col.g + col.g) / 3) * _GrayScaleOn) + (col.rgb * abs(_GrayScaleOn - 1));

				//Alternate way to make black and white
				//col.rgb = dot(col.rgb, float3(0.3, 0.59, 0.11));

				//check world position
				if(i.worldPos.y > _ScanPos)
					//top texture
					return col + noiseTop + (_Color * _Color.a);
				else
					//bottom texture
					return col + noiseBottom + (_Color * _Color.a);
			}
			ENDCG
		}
	}
	FallBack "Standard"
	CustomEditor "SEMCameraShaderGUI"
}
