// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Cody/DistanceCutOut/DistanceCutOut"
{
    Properties
    {
    	_Color("Color", Color) = (1,1,1,1)
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
		[Normal]_NormalMap ("Normal", 2D) = "bump" {}
        _Distance ("Distance To Fade", Range(0.1, 2)) = 0.5

        [Space(10)]
		[Header(Lighting Settings)]
        [KeywordEnum(Unlit, Lit)]_Lighting ("Lighting", Range(0,1)) = 1
        _LightIntensity("Light Intensity", Range(0.01, 1)) = 0.5
        _ShadowIntensity("Shadow Intensity", Range(0, 5)) = 1
        _Shininess("Shininess", Range(0.001,1)) = 1

        [Space(10)]
        [Header(Other)]
        [KeywordEnum(Off, Front, Back)]_CullMode ("Cull Mode", Range(0,2)) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _Blend2("Blend mode 1", float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendMode("Blend mode 2", float) = 10
        [IntRange]_Test("Int Test", Range(0,10)) = 1
    }
    SubShader
    {
        Tags {"Queue"="Geometry+1" "RenderType"="Transparent" "PreviewType"="Sphere"}
 
        Blend [_Blend2] [_BlendMode]
        Cull [_CullMode]

        Pass
        {
        	Tags {"LightMode"="ForwardBase"}

            CGPROGRAM
	            #pragma vertex vert
	            #pragma fragment frag
	            #include "UnityCG.cginc"

	            //Variables
	            uniform sampler2D _MainTex;
	            uniform float4 _MainTex_ST;
	            uniform sampler2D _NormalMap;
	            uniform float4 _NormalMap_ST;
	            uniform fixed4 _Color;
	            uniform float _Distance;
	            uniform fixed _Lighting;
	            uniform float _LightIntensity;
	            uniform float _ShadowIntensity;
	            uniform float _Shininess;

	            //Unity Variables
	            uniform float4 _LightColor0;

	            //Structs
	            struct vertexInput
	            {
	            	float4 vertex : POSITION;
	            	float3 normal : NORMAL;
	            	float4 texcoord : TEXCOORD0;
	            };
	            struct vertexOutput 
	            {
	                float4 pos : SV_POSITION;
	                float4 col : COLOR;
	                float2 uv : TEXCOORD0;
	                float4 worldPos : TEXCOORD1;
	                float3 normalDir : TEXCOORD2;
	            };

	            //Vertex
	            vertexOutput vert(vertexInput v) 
	            {
	                vertexOutput o;
	                _MainTex_ST = _NormalMap_ST;

	                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
	                o.pos = UnityObjectToClipPos(v.vertex);
	                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
	                return o;
	            }

	            //Fragment
	            fixed4 frag(vertexOutput i) : SV_Target 
	            {
            	  	float atten = 1.0;
	                float3 normalDirection = i.normalDir;
	                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
	                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

	                //Lighting
	                float3 diffuseReflection = atten * _LightColor0.xyz * _LightIntensity * max(0.0, dot(normalDirection, lightDirection));
	                float3 diffuseShadow = atten * _LightColor0.w * -_ShadowIntensity * max(-dot(normalDirection, lightDirection), 0.0);
	                float3 specularRefection = atten * _LightColor0.w * dot(normalDirection, lightDirection) * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), 1/_Shininess);
	                float3 finalLight = diffuseReflection + specularRefection + UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseShadow;
	                float4 lightingCol = float4(finalLight * _Lighting, 1.0);


	                //Distance Cut Off
	                fixed4 col = tex2D(_MainTex, i.uv) * _Color + lightingCol;
	                float dist = distance(i.worldPos, _WorldSpaceCameraPos);
	                col.a = saturate(dist / _Distance);
	                return col;
	            }
            ENDCG
        }
    }
    Fallback "Diffuse"
    CustomEditor "DistanceCutOffGUI"
}