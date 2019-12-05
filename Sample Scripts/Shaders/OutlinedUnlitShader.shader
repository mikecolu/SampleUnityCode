// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlinedUnlitShader"
{
    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,1)
        _Outline("Outline Thickness", Range(0.0, 0.3)) = 0.002
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
    }
    SubShader
    {
    	// Unlit
        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;
            half4 _Color;



            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_MainTex, i.uv);
                // col.rgb = lerp(_Color.rgb, col.rgb, _Color.a);
                return col * _Color;
            }
            ENDCG
        }

        // Outline
        Pass{

	        Tags {
				"RenderType"="Opaque"
				"Queue" = "Transparent"
			}

         	ColorMask RGB 
			Cull Front
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		 	#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			half _Outline;
			half4 _OutlineColor;
            

			struct appdata {
				half4 vertex : POSITION;
				half4 uv : TEXCOORD0;
				half3 normal : NORMAL;
				fixed4 color : COLOR;
			};

			struct v2f {
				half4 pos : POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

 
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				half2 offset = UnityObjectToWorldNormal(v.normal);
				o.pos.xy += offset * o.pos.z * _Outline;
				o.color = _OutlineColor;
				return o;
			}
 
			fixed4 frag(v2f i) : COLOR
			{
				fixed4 o;
				o = i.color;
				return o;
			}
			ENDCG
		}
    }
}