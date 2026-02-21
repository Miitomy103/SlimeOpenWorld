Shader "Custom/URP/ScreenDoorTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BayerTex ("BayerTex", 2D) = "black" {}
        _BlockSize ("BlockSize", Float) = 4
        _Radius ("Radius", Range(0.001, 100)) = 10
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
                float4 screenPos   : TEXCOORD2;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_BayerTex);
            SAMPLER(sampler_BayerTex);

            float4 _MainTex_ST;
            float _BlockSize;
            float _Radius;

            Varyings vert (Attributes v)
            {
                Varyings o;

                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionWS  = TransformObjectToWorld(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.screenPos = ComputeScreenPos(o.positionHCS);

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // カメラからの距離
                float dist = distance(i.positionWS, _WorldSpaceCameraPos);

                // 0〜1に正規化
                float clamp_distance = saturate(dist / _Radius);

                // スクリーンUV取得（0〜1）
                float2 screenUV = (i.screenPos.xy / i.screenPos.w);

                // ブロックサイズ分スケーリング
                float2 uv_BayerTex = screenUV * (_ScreenParams.xy / _BlockSize);

                // Bayer閾値取得
                float threshold = SAMPLE_TEXTURE2D(_BayerTex, sampler_BayerTex, uv_BayerTex).r;

                // 閾値未満なら破棄
                clip(clamp_distance - threshold);

                return col;
            }

            ENDHLSL
        }
    }
}
