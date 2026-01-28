Shader "Custom/GradientSkyboxVR"
{
    Properties
    {
        _Color2 ("Top Color", Color) = (0.97, 0.67, 0.51, 0)
        _Color1 ("Bottom Color", Color) = (0, 0.7, 0.74, 0)
        [Space]
        _Intensity ("Intensity", Range (0, 2)) = 1.0
        _Exponent ("Exponent", Range (0, 3)) = 1.0
        [HideInInspector]
        _Direction ("Direction", Vector) = (0, 1, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Pass
        {
            ZClip Off
            ZWrite Off
            Cull Off
            Fog { Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 direction : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float4 _Color1;
            float4 _Color2;
            float3 _Direction;
            float _Intensity;
            float _Exponent;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.pos = UnityObjectToClipPos(v.vertex);

                // Prendre la direction depuis la position du vertex pour la skybox
                o.direction = v.vertex.xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                // Pour Single Pass Instanced VR : ajuste la direction pour chaque œil
                #if defined(UNITY_SINGLE_PASS_STEREO)
                float3 dir = UnityStereoScreenSpaceToViewDir(i.pos.xy, i.pos.z);
                #else
                float3 dir = normalize(i.direction);
                #endif

                float d = dot(normalize(dir), _Direction) * 0.5f + 0.5f;
                return lerp(_Color1, _Color2, pow(d, _Exponent)) * _Intensity;
            }

            ENDCG
        }
    }
}
