Shader "GybeSkybox" {
	Properties {
		[SingleLineTexture] [Header(TEXTURES)] [Space(7)] [NoScaleOffset] _Tex1 ("Toon Sky (HDR)", Cube) = "black" {}
		[SingleLineTexture] [NoScaleOffset] _Tex3 ("Cloud (HDR)", Cube) = "black" {}
		[SingleLineTexture] [NoScaleOffset] _Tex2 ("Stars (HDR)", Cube) = "black" {}
		[Header(COLOR)] [Space(7) ] _ColorTop ("Color Top", Vector) = (0,1,0.827451,0)
		_ColorBottom ("Color Bottom", Vector) = (0.5843138,0,1,0)
		[Header(VALUES)] [Space(7) ] _Power ("Power", Float) = 1
		_Falloff ("Falloff", Float) = 0.6
		_HorizonHeight ("Horizon Height", Range(-1, 1)) = 0
		[Header(TOON SKY)] [Space(7)] [Toggle] _EnableToonSky ("Enable Toon Sky", Float) = 1
		[Toggle] _EnableToonSkyRotation ("Enable Toon Sky Rotation", Float) = 1
		_ToonSkyRotationSpeed ("Toon Sky Rotation Speed", Float) = 0.75
		_ToonySkyIntensity ("Toony Sky Intensity", Range(0, 1)) = 0.5
		[Header(SUN)] [Space(7)] [Toggle] _EnableSun ("Enable Sun", Float) = 1
		[Toggle] _EnableSunDirection ("Enable Sun Direction", Float) = 1
		[Toggle] _EnableHorizonMask ("Enable Horizon Mask", Float) = 1
		_SunSize ("Sun Size", Range(0, 1)) = 0.05
		_SunIntensity ("Sun Intensity", Range(0, 1)) = 0.2
		_SunRotationH ("Sun Rotation H", Range(0, 360)) = 0
		[Header(CLOUD)] [Space(7)] [Toggle] _EnableCloud ("Enable Cloud", Float) = 1
		[Toggle] _EnableCloudRotation ("Enable Cloud Rotation", Float) = 1
		_CloudRotationSpeed ("Cloud Rotation Speed", Float) = 1
		_CloudPosition ("Cloud Position", Range(-1, 1)) = 0
		_CloudIntensity ("Cloud Intensity", Range(0, 1)) = 1
		[Header(STARS)] [Space(7)] [Toggle] _EnableStars ("Enable Stars", Float) = 1
		[Toggle] _EnableStarsShine ("Enable Stars Shine", Float) = 1
		[Toggle] _EnableStarsRotation ("Enable Stars Rotation", Float) = 0
		_StarsRotationSpeed ("Stars Rotation Speed", Float) = 1
		_StarsShineSpeed ("Stars Shine Speed", Range(0, 1)) = 0.1
		_StarsIntensity ("Stars Intensity", Range(0, 1)) = 1
		[Header(FOG)] [Space(7)] [Toggle] _EnableFog ("Enable Fog", Float) = 0
		_FogIntensity ("Fog Intensity", Range(0, 1)) = 1
		_FogFill ("Fog Fill", Range(0, 1)) = 1
		_FogSmoothness ("Fog Smoothness", Range(0, 1)) = 0.01
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}