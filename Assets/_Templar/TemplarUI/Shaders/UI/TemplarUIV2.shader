// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/TemplarUIV2"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color1("start color", Color) = (1,1,1,1)
        _Color2("end color", Color) = (1,1,1,1)
        _UVOffset("UV offset", Vector) = (0,0,0,0)
        _UVScale("UV scale", Vector) = (1,1,1,0)


        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15
            _Emission("Emission", Float) = 0 // Added By Templar
            _EmissionTintOverride("Tint Override Emmision boost", Float) = 0 // Added By Templar
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
        [Toggle(SaturateForClip)] _SaturateForClip("SaturateForClip", Float) = 0 // Added By Templar
        [Toggle(TintOverride)] _DoSatOverride("Do Tint Saturation override", Float) = 0 // Added By Templar (When the source image color is black or white just add it to the color but when it is a very saturated color such as bright green then override the currect colors)
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
                #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
                #pragma multi_compile_local _ SaturateForClip // Added by Templar
                #pragma multi_compile_local _ TintOverride// Added by Templar

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                fixed4 _Color1;
                fixed4 _Color2;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float _Emission;
                float _EmissionTintOverride;
                uniform float3 _UVOffset;
                uniform float3 _UVScale;
                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                    OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    OUT.color = v.color;
                    return OUT;
                }
                float3 HSVToRGB(float3 c)
                {
                    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                    return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
                }


                float3 RGBToHSV(float3 c)
                {
                    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                    float d = q.x - min(q.w, q.y);
                    float e = 1.0e-10;
                    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
                }
                float clamp01(float a)
                {
                    return clamp(a, 0, 1);
                }
                float3 clampVec01(float3 a)
                {
                    a.x = clamp01(a.x);
                    a.y = clamp01(a.y);
                    a.z = clamp01(a.z);
                    return a;
                }
                float IsWhite(float3 a)
                {
                    return round((a.r+a.g+a.b)*0.25f);
                }
                float IsBlack(float3 a)
                {
                    return (1-round(saturate((a.r + a.g + a.b) * 10)));
                }
                fixed4 frag(v2f IN) : SV_Target
                {
                    //float2 uv = (IN.texcoord);
                    half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                    //float3 outRGBToHSV = RGBToHSV(color.rgb);
                    //float3 outHSVToRGB = HSVToRGB(float3(outRGBToHSV.r + uv.y, outRGBToHSV.g, outRGBToHSV.b));
                    //color.rgb = clampVec01(outHSVToRGB.rgb);
                    //float3 outHSVToRGB = HSVToRGB(float3((outRGBToHSV.x), outRGBToHSV.y, outRGBToHSV.z))

                    #ifdef SaturateForClip
                    color.a *= max(color.r,max(color.g,color.b));
                    #endif
                    #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                    #endif
                    #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                    #endif
                    

                    
                    float2 uv = (IN.texcoord * _UVScale) + (_UVOffset);
                    color = lerp(_Color1, _Color2, uv.y);

                    #ifdef TintOverride
                    float GreyscaleOfTint = (saturate(IN.color.r + IN.color.g + IN.color.b));
                    float BaseLerp = (1 - IsWhite(IN.color));
                    float LerpMod = 0.9f + (IsBlack(IN.color)*0.1f);//.9 for white and red 1 for black
                    float ColorLerp = BaseLerp * LerpMod;
                    color = lerp(color, IN.color, ColorLerp);
                    color += color * _EmissionTintOverride * ColorLerp * GreyscaleOfTint;
                    #endif
                    


                    //Bloom and Clamp Alpha
                    color += color * _Emission;
                    color.a = saturate(color.a);
                    return color;

                    //return color + (color * _Emission);
                }
            ENDCG
            }
        }
}