Shader "S6/UnlitTransparentAlwaysTop"
{
	Properties {
		_Color 		( "Color", Color )	= ( 1, 1, 1, 1 )
	}

	SubShader {
		Tags { "Queue"="Transparent+1" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		//Cull Front
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o)
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//_Color.a = 0.5f + _CosTime.a * 0.5f;
				return _Color;
			}
			ENDCG
		}
	}
}
