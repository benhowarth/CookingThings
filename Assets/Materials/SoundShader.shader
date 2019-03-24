Shader "Custom/SoundShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		
		_Speed ("Speed", Range(0,100)) = 10.0
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" "DisableBatching"="True"}
		GrabPass {
  			"_BackgroundTexture"
  			"_Time"
		}
		Pass{
			Ztest Always
			ZWrite On
		}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BackgroundTexture;

		

		
		struct Input {
			float2 uv_MainTex;
			float3 objPos;
			float3 worldPos;
		};
        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.objPos = v.vertex;
        }

		half _Glossiness;
		half _Metallic;
		float _Speed;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 st=IN.uv_MainTex*2-1;
			//float2 st=IN.uv_MainTex;
			float time=(sin(_Time*_Speed)+1)/2;
			
			float d=length(abs(IN.uv_MainTex)-.5);
			//float colorMod=step(length(st),0.5*time);
			//float colorMod=(frac(d*(time+0.5)*5.0)*0.7)-0.3;
			
			float colorMod=frac((d*(time+0.5)*15.0));
			
			
			colorMod+=smoothstep(length(abs(st)),0.9,1.1);
		
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			//fixed4 c = tex2D (_BackgroundTexture, IN.objPos) * _Color;
			
			fixed4 c = tex2D (_BackgroundTexture, IN.worldPos) * _Color;
			c=_Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a*colorMod;
			//o.Alpha=1;
		}
		ENDCG
	}
	FallBack "Transparent"
}
