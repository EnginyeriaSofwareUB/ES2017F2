// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SeaShader"
{
        Properties
    {
        // Color property for material inspector, default to white
		_MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _RandomOffset ("RandomOffset", float) = 1.0
        _Background("Background", int) = 0
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
            fixed _RandomOffset;
            fixed _Background;
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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float _half = (top + bottom)*0.5;
                float _diff = (bottom - top)*0.5;
                float maxY = (-abs(sin(i.uv.x*60+_Time.w/4.0+screenPos.y)))*0.8;
                if(_Background==1){
                	maxY = (-abs(sin(i.uv.x*60+sin(_Time.w/8.0+_RandomOffset)+screenPos.y)))*0.8;
                }
                fixed4 sum = fixed4(0.0h,0.0h,0.0h,0.0h);
                //sum = tex2D(_MainTex,float2(i.uv.x+(1-i.uv.y)*sin((_Time.w+i.uv.y)*2.3),i.uv.y))*screenPos.y;//*_SinTime.w
                if(_Background==1){
                sum = tex2D(_MainTex,float2(i.uv.x,(1-maxY)*i.uv.y));//*_SinTime.w
                if(sum.w<0.99){
                	sum = fixed4(0.0h,0.0h,0.0h,0.0h);
                }else{
                	sum/=2.0;
                	sum.w=1.0;
                }
                }else{
                	sum = tex2D(_MainTex,float2(i.uv.x,(1-maxY)*i.uv.y));//*_SinTime.w
	                if(sum.w<0.99){
	                	sum = fixed4(0.0h,0.0h,0.0h,0.0h);
	                }
                }

               	//sum = tex2D(_MainTex,float2(i.uv.x,i.uv.y))*screenPos.y;
                return sum;
            }
            ENDCG
        }
    }
}
