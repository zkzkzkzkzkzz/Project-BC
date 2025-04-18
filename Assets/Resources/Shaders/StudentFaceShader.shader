Shader "Custom/StudentFaceShader"
{
 Properties
    {
        _BaseTex("Base Texture", 2D) = "white" {}
        _MouthTex("Mouth Texture", 2D) = "white" {}
        _MouthMask("Mouth Mask", 2D) = "white" {}

        [Toggle]_UseMouth("Use Mouth", Float) = 1
        [IntRange]_Row("Row (U)", Range(0,7)) = 0
        [IntRange]_Col("Col (V)", Range(0,7)) = 0

        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 200
        Cull Back

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ _USEMOUTH_ON
            #include "UnityCG.cginc"

            sampler2D _BaseTex;
            sampler2D _MouthTex;
            sampler2D _MouthMask;
            float4 _BaseTex_ST;
            float4 _MouthMask_ST;

            int _Row;
            int _Col;
            float _Cutoff;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uvBase = TRANSFORM_TEX(i.uv, _BaseTex);
                float2 uvMask = TRANSFORM_TEX(i.uv, _MouthMask);

                fixed4 baseColor = tex2D(_BaseTex, uvBase);
                fixed4 mouthMask = tex2D(_MouthMask, uvMask);

                fixed4 finalColor = baseColor;
                float alpha = 1.0;

                #ifdef _USEMOUTH_ON
                {
                    float2 mouthCellSize = 0.125;  // 8x8 그리드

                    // 입 텍스처 좌상단 좌표
                    float2 mouthOffset;
                    mouthOffset.x = _Col * mouthCellSize;
                    mouthOffset.y = 1.0 - (_Row + 1) * mouthCellSize;

                    float2 mouthUV = (i.uv * 0.5) + mouthOffset;

                    fixed4 mouthColor = tex2D(_MouthTex, mouthUV);

                    float maskValue = saturate(mouthMask.r);
                    float mouthAlpha = mouthColor.a * maskValue;

                    finalColor = lerp(baseColor, mouthColor, mouthAlpha);
                    alpha = (1.0 - maskValue) + mouthAlpha;
                }
                #endif

                clip(alpha - _Cutoff);
                return finalColor;
            }
            ENDHLSL
        }
    }
}
