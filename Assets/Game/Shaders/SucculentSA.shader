
Shader "Unlit/SucculentSA"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;

            inline float4 UnityObjectToClipPos( in float3 pos )
            {
                #if defined(UNITY_SINGLE_PASS_STEREO) || defined(UNITY_USE_CONCATENATED_MATRICES)
                // More efficient than computing M*VP matrix product
                return mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(pos, 1.0)));
                #else
                return mul(UNITY_MATRIX_MVP, float4(pos, 1.0));
                #endif
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 sample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float progress = i.uv.x * _Time.y;
                clip(sample.a - 0.1);
                return sample;
            }
            ENDHLSL
        }
    }
}
