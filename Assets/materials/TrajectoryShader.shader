Shader "Unlit/TrajectoryShader"
{
	Properties
	{
		[HideInInspector] _Length ("Length", Float) = 1.0
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100


		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			half _Length;
			fixed3 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half c = i.uv.x*10;
				half d = i.uv.y;
				c = abs(c - round(c));
				d = abs(0.5 - d);

				// c : [0, 0.5]
				// d : [0, 0.5]

				c *= _Length/2;
				d *= 2;

				c*=5;
				d*=5;
				half ir2 = 1/(c*c + d*d);

				return fixed4(_Color, ir2);
			}
			ENDCG
		}
	}
}
