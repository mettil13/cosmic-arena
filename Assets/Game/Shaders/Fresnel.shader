Shader "Unlit/Fresnel"
{
    Properties
    {
        _PrimaryColor ("PrimaryColor", Color) = (1,1,1,1)
        _SecondaryColor ("SecondaryColor", Color) = (0,0,0,0)
        _FresnelPower ("FresnelPower", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _PrimaryColor;
            float4 _SecondaryColor;
            float _FresnelPower;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;
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


            float ProspetticFresnel(float3 worldPosition, float3 objectNormal){
                float3 V = normalize(_WorldSpaceCameraPos - worldPosition);
                return 1 - dot(V, objectNormal);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                return o;
            }

            void SG_Fresnel(float3 normal, float3 worldPos, float power, out float fresnel)
            {
                float3 N = normalize(normal);
                float3 V = normalize(_WorldSpaceCameraPos - worldPos);
                fresnel = pow(1-dot(V, N), power);
            }

            float4 frag (v2f i) : SV_Target
            {
                float fresnel;
                SG_Fresnel(i.normal, i.wPos, _FresnelPower, fresnel);

                return lerp(_SecondaryColor * 2, _PrimaryColor * 2, fresnel);
            }
            ENDHLSL
        }
    }
}
