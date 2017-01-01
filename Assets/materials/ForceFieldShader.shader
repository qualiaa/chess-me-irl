Shader "Unlit/ForceFieldShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[HideInInspector] _StartTime ("StartTime", Vector) = (0,0,0,0)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnityCG.cginc"

			//static const fixed RADIUS_SQR = 0.25;
			static const fixed RADIUS = 0.45;
			static const fixed RADIUS_SQR = 0.2025;
			static const fixed PI = 3.141592;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			// from http://answers.unity3d.com/questions/399751/randomity-in-cg-shaders-beginner.html
			float rand(float2 pos) {
             	return frac(sin(_Time.x * dot(pos ,float2(12.9898,78.233))) * 43758.5453);
         	}

         	float3 _StartTime;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.a = 0.5;

				// Calculate distance and angle from center 
				float2 pos = IN.texcoord.xy - 0.5;
				float r2 = (pos.x*pos.x + pos.y*pos.y);
				float th = atan2(pos.y/0.4,pos.x/0.4);

				// poor-man's atan2 [doesn't work :(]
				//float th = (1 + floor(pos.y*0.9)) * (acos(pos.x/RADIUS))
			 	//         + (1 - ceil(pos.y*0.9)) * (PI+acos(-pos.x/RADIUS));

			 	// Perturb radius with angle and time
				r2 +=  0.005*sin(th*2 + _Time.w) + rand(pos)/20;

				// Create circle-ish alpha mask
				float circleMask = (RADIUS_SQR - r2) * 100;
				circleMask = clamp(circleMask,0,1);

				// Shimmering blue colour
				c.rg = 0.7;
				c.b = 0.75;

				float p = 0.3 * sin(3.14159*pos.x/0.4 + _Time.x) * sin(3.141592*pos.y/0.4  + _Time.x*2) * _SinTime.w;
				c.b += p;

				// Fade
				float timeFade =      _StartTime.z  * (_Time.y - _StartTime.x)
							   + (1 - _StartTime.z) * (min(_StartTime.y - _StartTime.x,1) - (_Time.y - _StartTime.y));

				timeFade = clamp(timeFade,0,1);
				timeFade = log(timeFade+1);
				// Apply

				c.a *= circleMask * timeFade;//_SinTime.w/2 + 0.5;

				c.rgb *= c.a;
				return c;
			}

//			fixed4 frag(v2f IN) : SV_Target
//			{
//				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
//				c.a = 0.5;
//
//				float2 pos = abs(IN.texcoord.xy - 0.5);
//				float r = (pos.x*pos.x + pos.y*pos.y);
//				float th = atan2(pos.y,pos.x);
//				r +=  sin(th + _Time.w);

//				//float circle_mask = t.a;
//				float circle_mask = (RADIUS - r) * 1000;
//				circle_mask = clamp(circle_mask,0,1);

//				c.a *= circle_mask;//_SinTime.w/2 + 0.5;


//				c.rgb *= c.a;
//				return c;
//			}

		ENDCG
		}
	}}
