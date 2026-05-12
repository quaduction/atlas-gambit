Shader "AtlasGambit/Veil"
{
    Properties
    {
        [Header(Base Settings)]
        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.08
        _DistortionScale ("Distortion Scale", Range(0, 1)) = 0.5
        _DistortionSpeed ("Distortion Speed", Range(0, 2)) = 0.5
        
        [Header(Chromatic Aberration)]
        _ChromaticAberration ("Chromatic Aberration", Range(0, 0.05)) = 0.2
        
        [Header(Visuals)]
        _Tint ("Glass Tint", Color) = (1, 1, 1, 0.2)
        _Glossiness ("Glossiness/Specular", Range(0, 1)) = 0.8
    }

    SubShader
    {
        // Transparent queue to allow the tint to overlay the background
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Required for VR Single Pass Instanced
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID // VR Support
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO // VR Support
            };

            float _DistortionStrength;
            float _DistortionScale;
            float _DistortionSpeed;
            float _ChromaticAberration;
            float4 _Tint;
            float _Glossiness;

            // Pseudo-noise function to simulate undulating glass without needing a texture
            float2 GetWaveOffset(float2 uv, float time)
            {
                float2 offset;
                offset.x = sin(uv.y * 10.0 * _DistortionScale + time) * _DistortionStrength;
                offset.y = cos(uv.x * 10.0 * _DistortionScale + time) * _DistortionStrength;
                return offset;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Initialize VR Stereo ID
                UNITY_SETUP_INSTANCE_ID(input);
                // UNITY_TRANSFER_INSTANCE_ID(input, output); // explodes
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = input.uv;

                // Calculate Screen Position for refraction lookup
                float4 pos = ComputeScreenPos(output.positionCS);
                output.screenPos = pos;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Initialize VR Stereo ID in fragment shader
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // Calculate UVs for the opaque texture
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                
                // Create the water/glass effect using sine waves
                float time = _Time.y * _DistortionSpeed;
                float2 distortion = GetWaveOffset(input.uv, time);

                // --- CHROMATIC ABERRATION ---
                // We sample the Opaque Texture 3 times with slightly different offsets
                
                // Red Channel: Offset outwards
                float2 uvR = screenUV + (distortion * (1.0 + _ChromaticAberration));
                half3 colorR = SampleSceneColor(uvR);

                // Green Channel: No offset (the anchor)
                float2 uvG = screenUV + distortion;
                half3 colorG = SampleSceneColor(uvG);

                // Blue Channel: Offset inwards
                float2 uvB = screenUV + (distortion * (1.0 - _ChromaticAberration));
                half3 colorB = SampleSceneColor(uvB);

                // Combine channels
                half3 finalRefraction = half3(colorR.r, colorG.g, colorB.b);

                // Apply the glass tint and opacity
                half4 finalColor = half4(finalRefraction * _Tint.rgb, _Tint.a);

                return finalColor;
            }
            ENDHLSL
        }
    }
}