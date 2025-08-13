Shader "Custom/CoinCut_Z_Textures"
{
    Properties
    {
        _F_Tex ("F Side Texture", 2D) = "white" {}
        _P_Tex ("P Side Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float4 pos : SV_POSITION;
                float zpos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            sampler2D _F_Tex;
            sampler2D _P_Tex;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.zpos = v.vertex.z; // Axe Z local du mesh
                o.uv = v.uv;
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                if (i.zpos < 0)
                    return tex2D(_F_Tex, i.uv);
                else
                    return tex2D(_P_Tex, i.uv);
            }
            ENDCG
        }
    }
}
