Shader "Cody/StencilMask" 
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", color) = (1,1,1,1)
		_StencilRef("Stencil Ref", int) = 1
	}
	  SubShader 
	  {
        Tags { "RenderType"="Transparent" }
        Stencil {
            Ref [_StencilRef]
            Comp always
            Pass replace
        }
 
        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
		
        struct Input {
            fixed3 Albedo;
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
         {
        	fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}