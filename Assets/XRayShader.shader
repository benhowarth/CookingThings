Shader "Custom/XRayShader"
{
	Properties
	{
		_MainTex("Texture",2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_XRayColor ("XRayColor", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{"RenderType"="Opaque" "Queue"="Geometry+1"}
		//behind something
		Pass{
			ZWrite Off
			ZTest Greater
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct input{
				float4 vertex: POSITION;
				float2 uv: TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv: TEXCOORD0;
				float4 vertex: SV_POSITION;
			};
			v2f vert(input v)
			{
				v2f o;
				o.vertex=UnityObjectToClipPos(v.vertex);
				o.uv=v.uv;
				return o;
			}
			fixed4 _XRayColor;
			fixed4 frag(v2f i) : SV_Target
			{
				//fixed4 col=float4(0.4,0.0,0.0,0.35);
				fixed4 col=_XRayColor;
				return col;
			}
			ENDCG
			
		}
		//In front of things
		Pass{
			ZTest Less
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct input{
				float4 vertex: POSITION;
				float2 uv: TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv: TEXCOORD0;
				float4 vertex: SV_POSITION;
			};
			v2f vert(input v)
			{
				v2f o;
				o.vertex=UnityObjectToClipPos(v.vertex);
				//o.vertex=mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV,v.uv));
				o.uv=v.uv;
				return o;
			}
			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col=tex2D(_MainTex,i.uv)*_Color;
				return col;
			}
			ENDCG
			
		}
	}
	Fallback "Diffuse"
}