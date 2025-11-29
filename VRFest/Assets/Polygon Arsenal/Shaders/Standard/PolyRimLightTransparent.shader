Shader "PolygonArsenal/PolyRimLightTransparent"
{
    Properties
    {
        _InnerColor ("Inner Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimWidth ("Rim Width", Range(0.2,20.0)) = 3.0
        _RimGlow ("Rim Glow Multiplier", Range(0.0,9.0)) = 1.0
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent" 
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Cull Back
        Lighting Off
        Blend One One
        ZWrite Off
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };
            
            CBUFFER_START(UnityPerMaterial)
                float4 _InnerColor;
                float4 _RimColor;
                float _RimWidth;
                float _RimGlow;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionHCS = vertexInput.positionCS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // Нормализуем векторы
                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = normalize(input.viewDirWS);
                
                // Вычисляем рим-эффект
                half rim = 1.0 - saturate(dot(viewDirWS, normalWS));
                half3 rimEffect = _RimColor.rgb * _RimGlow * pow(rim, _RimWidth);
                
                // Комбинируем внутренний цвет с рим-эффектом
                half3 finalColor = _InnerColor.rgb + rimEffect;
                
                return half4(finalColor, _InnerColor.a);
            }
            ENDHLSL
        }
    }
    
    Fallback "Universal Render Pipeline/Unlit"
}