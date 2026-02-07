#ifndef TOON_FORWARD_PASS_INCLUDE
#define TOON_FORWARD_PASS_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

//
struct Attributes
{
    float4 positionOS  : POSITION;
    float2 texcoord    : TEXCOORD0;
    float3 normalOS    : NORMAL;
};
struct Varyings
{
    float2 uv          : TEXCOORD0;
    float3 normalWS    : TEXCOORD1;
    float3 positionWS  : TEXCOORD3;
    float3 viewDirWS   : TEXCOORD2;
    float4 positionCS  : SV_POSITION;
};


///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////
Varyings UnlitVertex(Attributes IN)
{
    Varyings OUT = (Varyings)0;

    VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS);
    VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS);

    OUT.uv = TRANSFORM_TEX(IN.texcoord, _BaseTex);
    OUT.normalWS = normalInput.normalWS;
    OUT.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
    OUT.positionWS = vertexInput.positionWS;
    OUT.positionCS = vertexInput.positionCS;

    return OUT;
}

half4 UnlitFragment(Varyings IN) : SV_Target
{
    half4 baseColor = SAMPLE_TEXTURE2D(_BaseTex, sampler_BaseTex, IN.uv);

    float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
    Light mainLight = GetMainLight(shadowCoord);

    float3 N = normalize(IN.normalWS);
    float3 V = normalize(IN.viewDirWS);
    float3 L = normalize(mainLight.direction);

    float NdotL = dot(N, L);
    float NdotV = dot(N, V);
 

    half shadow = MainLightRealtimeShadow(shadowCoord);

    half inShadow = step(NdotL * shadow, 0.3);

    half3 shadowColor = baseColor.rgb * _ShadowColor.rgb * mainLight.color;
    half3 unlitColor = baseColor.rgb * mainLight.color;

    half3 color = lerp(unlitColor, shadowColor, inShadow);

    // Rim Light
    half rim = 1.0 - saturate(NdotV);
    half back = saturate(NdotL);

    half rimLight = lerp(pow(rim * back, _RimStrength), 0, inShadow);
    color.rgb += rimLight * mainLight.color * _RimColor.rgb;

    return half4(color, baseColor.a);
}
#endif
