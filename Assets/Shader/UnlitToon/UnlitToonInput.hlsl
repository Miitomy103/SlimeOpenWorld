#ifndef UNLIT_TOON_INPUT_INCLUDE
#define UNLIT_TOON_INPUT_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

TEXTURE2D(_BaseTex); SAMPLER(sampler_BaseTex);

CBUFFER_START(UnityPerMaterial)
    float4 _BaseTex_ST;

    float _OutlineWidth;
    half4 _OutlineColor;

    half4 _ShadowColor;

    half4 _RimColor;
    float _RimThreshold;
    float _RimStrength;
CBUFFER_END

#endif
