Shader "Custom/LitTransparentDoubleSided"
{
    Properties
    {
        _Color ("Albedo Color", Color) = (1,1,1,1) // Albedo (R,G,B,A)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha // 透明処理

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
            };

            fixed4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                if (_WorldSpaceLightPos0.w == 0) // **ライトがない場合**
                {
                    lightDir = float3(0, 1, 0); // **上方向の仮のライト**
                }

                float diff = max(dot(i.normal, lightDir), 0) + 0.1;
                fixed3 color = _Color.rgb * diff;
                return fixed4(color, _Color.a);
            }
            ENDCG
        }
    }
}