﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TileShader"
{
        Properties
    {
        // Color property for material inspector, default to white
		_MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _SkyColor ("SkyColor", Color) = (0.23,0.062,0.482)
        _SandColor ("SandColor", Color) = (1,0.957,0.58)
    }
    SubShader
    {
    	Tags { "Queue"="Transparent" }
        Pass
        {
        	ZTest LEqual
        	ZWrite Off
        	//ZTest Always
        	Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _TexCoords;
            fixed4 _Color;
            fixed4 _SkyColor;
            fixed4 _SandColor;
            fixed _RandomStart;
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

            fixed4 frag (v2f i) : SV_Target
            {
                
                //float offsett = i.position.x*i.position.y;//Llamas rotas
                //float offsett = i.position.x+i.position.y;//Llamas ralladas
                float offsett = _TexCoords.x;

                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float _half = (top + bottom)*0.5;
                float _diff = (bottom - top)*0.5;
                screenPos.x = screenPos.x*(_half+_diff*screenPos.y);
                screenPos.x = (screenPos.x+1)*0.5;
                screenPos.y = (screenPos.y+1)*0.5;
                fixed4 sum = fixed4(0.0h,0.0h,0.0h,0.0h);
                //sum = tex2D(_MainTex,float2(i.uv.x+(1-i.uv.y)*sin((_Time.w+i.uv.y)*2.3),i.uv.y))*screenPos.y;//*_SinTime.w

                //sum = tex2D(_MainTex,float2(i.uv.x+i.uv.y/8*sin((_Time.z+(1-i.uv.y))*2.3+_RandomStart),i.uv.y));//*_SinTime.w
               	//sum = tex2D(_MainTex,float2(i.uv.x,i.uv.y))*_SandColor*screenPos.y + _SkyColor*(1-screenPos.y);
				sum = tex2D(_MainTex,float2(i.uv.x,i.uv.y))*_SandColor;
               	sum.w = 1.0h;
                return sum;
            }
            ENDCG
        }
    }
}
