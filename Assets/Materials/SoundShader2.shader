Shader "Unlit/SoundShader2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"DisableBatching"="True"
		}
		LOD 100
		GrabPass{
			"_BackgroundTexture"
		}

		Pass
		{
			ZTest Always
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			sampler2D _BackgroundTexture;
			float _Speed;

			struct vertexIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct vertexOut
			{
				float4 pos : SV_POSITION;
				float4 grabPos : TEXCOORD0;
			};


			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			vertexOut vert(vertexIn input)
			{
			  vertexOut output;
			  float4 pos = input.vertex;
			  pos=mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV,float4(0,0,0,1))+float4(pos.x,pos.z,0,0));
			  output.pos=pos;
			  output.grabPos=ComputeGrabScreenPos(output.pos);
			  output.grabPos.x+=cos(_Time.x*_Speed);
			  output.grabPos.y+=sin(_Time.x*_Speed);
			  return output;
			}

			float4 frag(vertexOut input) : COLOR
			{
			  //return float4(1,1,1,1);
              return tex2Dproj(_BackgroundTexture, input.grabPos);
			}
			ENDCG
		}
	}
}
