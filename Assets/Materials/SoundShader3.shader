Shader "Unlit/SoundShader3"
{
	Properties
	{
		_Speed("Speed", float) = 1.0
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


			
			vertexOut vert(vertexIn input)
			{
				vertexOut output;
				float noise=1;
				float4 pos = input.vertex;
				//pos=mul(UNITY_MATRIX_P,UnityObjectToViewPos(float4(0,0,0,1))+float4(pos.x,pos.z,0,0));
				
			  	pos=mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV,float4(0,0,0,1))+float4(pos.x,pos.z,0,0));
				output.pos=pos;
				output.grabPos=ComputeGrabScreenPos(output.pos);




				float2 st=input.uv*2-1;
				//float2 st=IN.uv_MainTex;
				float time=(sin(_Time*_Speed)+1)/2;

				float d=length(abs(input.uv)-.5);
				//float colorMod=step(length(st),0.5*time);
				float colorMod=(frac(d*(time+0.5)*5.0)*0.7)-0.3;


				colorMod+=smoothstep(length(abs(st)),0.5,0.7);
				


				output.grabPos.x+=colorMod;
				output.grabPos.y+=colorMod;
				return output;
			}

			float4 frag(vertexOut input) : COLOR
			{
              return tex2Dproj(_BackgroundTexture, input.grabPos);
			  //return float4(1,1,1,1);
			}
			ENDCG
		}
	}
}
