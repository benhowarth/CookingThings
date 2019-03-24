Shader "Custom/WallShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Overlay+1"
			"DisableBatching"="True"
		}
		LOD 100
		GrabPass{
			"_BackgroundTexture"
		}

		Pass
		{
			ZTest Equal
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			sampler2D _BackgroundTexture;
			sampler2D _MainTex;

			struct vertexIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct vertexOut
			{
				float4 pos : SV_POSITION;
				float4 grabPos : TEXCOORD0;
				float4 col: COLOR;
			};


			
			vertexOut vert(vertexIn input)
			{
				vertexOut output;
				
				float4 pos = input.vertex;
				pos=mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV,float4(pos.x,pos.y,pos.z,1)));
				output.pos=pos;
				//output.grabPos=ComputeGrabScreenPos(output.pos);

				
				return output;
			}

			float4 frag(vertexOut input) : COLOR
			{
				return tex2D(_MainTex,input.pos);
				//return tex2Dproj(_BackgroundTexture, input.grabPos);
			  			  //return float4(1,1,1,1);
			}
			ENDCG
		}
	}
}
