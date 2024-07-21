Shader "Custom/SimplifiedPBRShaderHDRP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _BaseMap ("Base Map", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicMap ("Metallic Map", 2D) = "white" {}
        _Roughness ("Roughness", Range(0.0, 1.0)) = 0.5
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _OcclusionMap ("Occlusion Map", 2D) = "white" {}
    }

    SubShader
    {
        Tags{"RenderPipeline" = "HDRenderPipeline"}
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            float _Metallic;
            TEXTURE2D(_MetallicMap);
            SAMPLER(sampler_MetallicMap);

            float _Roughness;
            TEXTURE2D(_RoughnessMap);
            SAMPLER(sampler_RoughnessMap);

            TEXTURE2D(_OcclusionMap);
            SAMPLER(sampler_OcclusionMap);
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformWorldToHClip(input.vertex.xyz);
                output.positionWS = TransformObjectToWorld(input.vertex.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normal);
                output.uv = input.uv;
                float3x3 tangentToWorld = float3x3(
                    TransformObjectToWorldDir(input.tangent.xyz),
                    cross(output.normalWS, TransformObjectToWorldDir(input.tangent.xyz)) * input.tangent.w,
                    output.normalWS);
                output.tangentWS = tangentToWorld[0];
                output.bitangentWS = tangentToWorld[1];
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float3 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).rgb * _BaseColor.rgb;
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv));
                float3 normalWS = normalize(input.tangentWS * normalTS.x + input.bitangentWS * normalTS.y + input.normalWS * normalTS.z);

                float metallic = SAMPLE_TEXTURE2D(_MetallicMap, sampler_MetallicMap, input.uv).r * _Metallic;
                float roughness = SAMPLE_TEXTURE2D(_RoughnessMap, sampler_RoughnessMap, input.uv).r * _Roughness;
                float occlusion = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, input.uv).r;

                // Simple lighting model for PBR
                float3 lightDir = normalize(float3(0.5, 0.5, 0.5));
                float3 viewDir = normalize(input.positionWS - _WorldSpaceCameraPos);
                float3 halfDir = normalize(lightDir + viewDir);
                float NdotL = saturate(dot(normalWS, lightDir));
                float NdotV = saturate(dot(normalWS, viewDir));
                float NdotH = saturate(dot(normalWS, halfDir));
                float VdotH = saturate(dot(viewDir, halfDir));

                float3 F0 = lerp(float3(0.04, 0.04, 0.04), albedo, metallic);
                float3 F = F0 + (1.0 - F0) * pow(1.0 - VdotH, 5.0);
                float D = roughness * roughness / (PI * pow(NdotH * (roughness * roughness - 1.0) + 1.0, 2.0));
                float k = roughness / 2.0;
                float G = NdotL * NdotV / (NdotL * (1.0 - k) + k) / (NdotV * (1.0 - k) + k);

                float3 numerator = D * G * F;
                float denominator = 4.0 * NdotL * NdotV + 0.001; // Prevent division by zero
                float3 specular = numerator / denominator;

                float3 kS = F;
                float3 kD = 1.0 - kS;
                kD *= 1.0 - metallic;

                float3 diffuse = albedo / PI;
                float3 color = (kD * diffuse + specular) * NdotL * occlusion;

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "HDRP/Lit"
}
