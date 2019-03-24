Shader "Unlit/SoundShader4"
{
	Properties
	{
		_Speed("Speed", float) = 1.0
		_Strength("Strength", float) = 0.2
		_MaxRad("Radius",float)=0.0
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Overlay"
			"DisableBatching"="True"
		}
		LOD 1000
		GrabPass{
			"_BackgroundTexture"
		}

		Pass
		{
			ZTest Always
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			sampler2D _BackgroundTexture;
			float _Speed;
			float _Strength;
			float _MaxRad;

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
				//pos=mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV,float4(0,0,0,1))+float4(pos.x,pos.z,0,0));
				pos=mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV,float4(pos.x,pos.y,pos.z,1)));
				output.pos=pos;
				output.grabPos=ComputeGrabScreenPos(output.pos);




				float2 st=input.uv*2-1;
				float time=(sin(_Time*_Speed)+1)/2;

				float d=length(abs(st)-.5);
				//float colorMod=(frac(d*(time+0.5)*5.0)*0.7)-0.3;
			
				float colorMod=frac((d*(time+0.5)*15.0));
				
				//warp
				
				//dampen warp
				colorMod*=_Strength;
				
				//dampen warp over time too
				//colorMod/=_Time;
				
				//make sure warp only in circle shape
				//colorMod*=step(length(st),_MaxRad);
				colorMod*=step(length(st),0.5);
				
				
				//take out centre of warp to make a ring
				colorMod*=(1-step(length(st),0.3));
				

				//colorMod+=smoothstep(length(st),0.0,0.5);
				//colorMod*=0.3;
				
				
				output.grabPos.x+=colorMod*-sign(pos.x);
				output.grabPos.y+=colorMod*-sign(pos.y);
				output.col=float4(colorMod,0.0,0.0,1.0);
				
				return output;
			}

			float4 frag(vertexOut input) : COLOR
			{
              return tex2Dproj(_BackgroundTexture, input.grabPos)+input.col;
			  //return float4(1,1,1,1);
			}
			ENDCG
		}
	}
}
