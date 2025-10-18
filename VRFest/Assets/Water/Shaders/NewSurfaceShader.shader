Shader "Sprites/PanningDiffuseURP"
{
    Properties
    {
        [MainTexture] _MainTex("Sprite Texture", 2D) = "white" {}
        [MainColor] _Color("Tint", Color) = (1,1,1,1)
        _Speed("Speed", float) = 0.0
        [Toggle] _PixelSnap ("Pixel snap", Float) = 0
        
        // URP required properties
        _Surface("Surface Type", Float) = 0.0
        _Blend("Blend Mode", Float) = 0.0
        _Cull("Cull Mode", Float) = 0.0
        _AlphaClip("Alpha Clipping", Float) = 0.0
        _SrcBlend("Src Blend", Float) = 1.0
        _DstBlend("Dst Blend", Float) = 0.0
        _ZWrite("Z Write", Float) = 0.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ _PIXELSNAP_ON
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                half4 color         : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                half4 color         : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                float _Speed;
                float _PixelSnap;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;

                #ifdef _PIXELSNAP_ON
                output.positionHCS = UnityPixelSnap(output.positionHCS);
                #endif

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                
                // Apply panning
                float2 pannedUV = input.uv;
                pannedUV.x -= _Time.y * _Speed;
                
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, pannedUV) * input.color;
                
                // Apply premultiplied alpha for correct blending
                color.rgb *= color.a;
                
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "Universal Render Pipeline/2D/Sprite-Lit-Default"
}