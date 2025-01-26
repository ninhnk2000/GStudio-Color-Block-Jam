// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlineShader" {
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Texture",2D) = "white"{}
		_OutlineWidth("Outline Width", float) = 0.1
		_OutlineColor("Outline Color",Color) = (1,1,1,1)
	}

	SubShader {
		Pass {
			Cull Front
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            uniform half4 _OutlineColor;
            uniform float _OutlineWidth;

            struct vertexInput {
            float4 position: POSITION;
            float4 texcoord: TEXCOORD0;
            };

            struct vertexOutput {
            float4 position: SV_POSITION;
            float4 texcoord: TEXCOORD0;
            };

            float4 Outline(float4 vertPos, float width)
			{
				float4x4 scaleMat;
				scaleMat[0][0] = 1.0 + width;
				scaleMat[0][1] = 0.0;
				scaleMat[0][2] = 0.0;
				scaleMat[0][3] = 0.0;
				scaleMat[1][0] = 0.0;
				scaleMat[1][1] = 1.0 + width;
				scaleMat[1][2] = 0.0;
				scaleMat[1][3] = 0.0;
				scaleMat[2][0] = 0.0;
				scaleMat[2][1] = 0.0;
				scaleMat[2][2] = 1.0 + width;
				scaleMat[2][3] = 0.0;
				scaleMat[3][0] = 0.0;
				scaleMat[3][1] = 0.0;
				scaleMat[3][2] = 0.0;
				scaleMat[3][3] = 1.0;	

				return mul(scaleMat, vertPos);
			}

            vertexOutput vert(vertexInput v) 
            {
                vertexOutput o;
                o.position = UnityObjectToClipPos(Outline(v.position,_OutlineWidth));
                return o;
            }

            half4 frag(vertexOutput i): COLOR 
            {
            return _OutlineColor;
            }

			ENDCG
		}
	}
}

