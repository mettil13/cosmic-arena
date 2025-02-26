Shader "Unlit/MrMarvelousAbility"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _SecondaryColor ("SecondaryColor", Color) = (1,1,1,1)
        _Speed ("Speed", Float) = 7
        _Thickness ("Thickness", Range(0, 0.49)) = 0.45
        _BorderThickness ("BorderThickness", Float) = 0
        _ExternalBorder ("ExternalBorder", Float) = 0
        _BorderColor ("BorderColor", Color) = (1,1,1,1)
        _Frequency ("Frequency", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        Pass
        {

            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha 

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _Color;
            float4 _SecondaryColor;
            float _Speed;
            float _Thickness;
            float _BorderThickness;
            float _ExternalBorder;
            float4 _BorderColor;
            float _Frequency;

            #define PI 3.14159265359
            #define TAU 6.28318530718

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            inline float4 UnityObjectToClipPos( in float3 pos )
            {
                #if defined(UNITY_SINGLE_PASS_STEREO) || defined(UNITY_USE_CONCATENATED_MATRICES)
                // More efficient than computing M*VP matrix product
                return mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(pos, 1.0)));
                #else
                return mul(UNITY_MATRIX_MVP, float4(pos, 1.0));
                #endif
            }

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolator i) : SV_Target
            {
                float2 remapUV = i.uv * 2 - 1;
                float x = (length(remapUV) * _Frequency + _Time.y * _Speed - TAU) % PI;
                float h = saturate(cos(x) * 0.5 - _Thickness);
                h = h / (0.5-_Thickness);
                float internalBorder = length(remapUV) > _ExternalBorder - _BorderThickness;
                float externalBorder = length(remapUV) < _ExternalBorder;
                clip(externalBorder - 0.1);
                return lerp(lerp(_SecondaryColor, _Color, h), _BorderColor, internalBorder);
            }
            ENDHLSL
        }
    }
}
