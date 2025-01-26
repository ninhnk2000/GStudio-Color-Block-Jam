Shader "CW/RimOpaque" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Color1 ("Color 1", Vector) = (1,0.5,0.5,1)
		_Color2 ("Color 2", Vector) = (0.5,0.5,1,1)
		_Rim ("Rim", Float) = 1
		_Shift ("Shift", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}