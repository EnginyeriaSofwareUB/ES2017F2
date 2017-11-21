﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TileDepthShader"
{
        Properties
    {
        // Color property for material inspector, default to white
		_MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _TexCoords;
            fixed4 _Color;
            float top;
            float bottom;

            struct data {
            	float4 vertex : POSITION;
            	float2 uv: TEXCOORD0;
            };

            struct v2f{
            	float4 position: SV_POSITION;
            	float4 screenPos: TEXCOORD0;
            	float2 uv:TEXCOORD1;
            };

            v2f vert(data i){
            	v2f o;
            	o.position = UnityObjectToClipPos(i.vertex);
            	o.uv = TRANSFORM_TEX(i.uv,_MainTex);
            	o.screenPos = o.position;
            	return o;
            }

            half4 frag (v2f i) : COLOR
            {
                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float _half = (top + bottom)*0.5;
                float _diff = (bottom - top)*0.5;
                screenPos.x = screenPos.x*(_half+_diff*screenPos.y);
                screenPos.x = (screenPos.x+1)*0.5;
                screenPos.y = (screenPos.y+1)*0.5;
                half4 sum = half4(0.0h,0.0h,0.0h,0.0h);
                sum = tex2D(_MainTex,float2(i.uv.x+(1-i.uv.y)*sin((_Time.w+i.uv.y)*2.3),i.uv.y))*screenPos.y;//*_SinTime.w
                //sum = tex2D(_MainTex,float2(i.uv.x+i.uv.y*sin((_Time.w+(1-i.uv.y))*2.3),i.uv.y));//*_SinTime.w
               	//sum = tex2D(_MainTex,float2(i.uv.x,i.uv.y))*screenPos.y;
                return sum;
            }
            ENDCG
        }
    }
}
