Shader "Unlit/CameraDistanceFade"
{
    Properties
    {
        _Distance("Distance",float) = 1
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100
        blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
                float3 positionWS : TEXCOORD1;
            };

            float _Distance;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.positionWS = TransformObjectToWorld(v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = TransformObjectToHClip(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 cameraPositionWs = _WorldSpaceCameraPos.xyz;

                float distance = length(cameraPositionWs - i.positionWS);
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                float coefficient = saturate(distance * _Distance);
                col.a *= coefficient;
                return col;
            }
            ENDHLSL
        }
    }
}