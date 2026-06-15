Shader "Custom/FrogImageRecolor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BodyColor ("Body Color", Color) = (1,0,0,1)
        _PatternColor ("Pattern Color", Color) = (0,0,1,1)
        _Threshold ("Threshold", Float) = 0.5
        _Greyscale ("Greyscale", Float) = 0  // add this
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MainTex;
            float4 _BodyColor;
            float4 _PatternColor;
            float _Threshold;
            float _Greyscale;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float maxChannel = max(col.r, max(col.g, col.b));
                
                // ignore very dark pixels
                float darkness = step(0.15, maxChannel);

                // stricter dominant channel detection
                float isRed = step(col.b * 1.5, col.r) * step(col.g * 1.5, col.r) * darkness;
                float isBlue = step(col.r * 1.5, col.b) * step(col.g * 1.5, col.b) * darkness;

                // preserve shading by scaling new color by original brightness
                float brightness = maxChannel;

                fixed4 bodyResult = _BodyColor * brightness;
                bodyResult.a = col.a;
                
                fixed4 patternResult = _PatternColor * brightness;
                patternResult.a = col.a;

                fixed4 result = col;
                result = lerp(result, bodyResult, isRed);
                result = lerp(result, patternResult, isBlue);


                if (_Greyscale > 0.5)
                {
                    float grey = dot(result.rgb, float3(0.299, 0.587, 0.114));
                    result.rgb = float3(grey, grey, grey);
                }

                return result;
            }
            ENDCG
        }
    }
}