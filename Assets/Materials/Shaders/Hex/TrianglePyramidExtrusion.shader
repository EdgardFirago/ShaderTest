Shader "Geometry/TrianglePyramidExtrusion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ExtrusionFactor("Extrusion factor", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
 
            #include "UnityCG.cginc"
            #include "UnityGBuffer.cginc"
            #include "UnityStandardUtils.cginc"

 
            struct appdata
            {
                float4 position : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
 
            struct v2g
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
 
            struct g2f
            {
                float4 worldPos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color: Color;

            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ExtrusionFactor;
            float _LocalTime;
 
            v2g vert (appdata v)
            {
                v2g o;
                o.position = v.position;
                o.uv = v.uv;
                o.normal = v.normal;
                return o;
            }
            
            float3 ConstructNormal(float3 v1, float3 v2, float3 v3)
            {
                return normalize(cross(v2 - v1, v3 - v1));
            }

            g2f VertexOutput(float3 wpos, float2 uv, fixed4 color)
            {
                g2f o;
                o.worldPos = UnityObjectToClipPos(wpos);
                UNITY_TRANSFER_FOG(o, o.worldPos);
                o.uv = TRANSFORM_TEX(uv, _MainTex);
                o.color = color;
                return o;
            }


            [maxvertexcount(15)]
            void geom(triangle v2g IN[3],inout TriangleStream<g2f> triStream)
            {
                
                g2f o;
                
                float3 wp0 = IN[0].position.xyz;
                float3 wp1 = IN[1].position.xyz;
                float3 wp2 = IN[2].position.xyz;


                float3 offs = ConstructNormal(wp0, wp1, wp2) * _ExtrusionFactor;
                float3 wp3 = wp0 + offs;
                float3 wp4 = wp1 + offs;
                float3 wp5 = wp2 + offs;
                triStream.Append(VertexOutput(wp3, IN[0].uv, fixed4(0, 0, 0, 1)));
                triStream.Append(VertexOutput(wp4, IN[1].uv, fixed4(0, 0, 0, 1)));
                triStream.Append(VertexOutput(wp5, IN[2].uv, fixed4(0, 0, 0, 1)));
                triStream.RestartStrip();



                //edge 
                float3 wn = ConstructNormal(wp3, wp4, wp5);
                float4 wt = float4(normalize(wp3 - wp0), 1);

                triStream.Append(VertexOutput(wp3, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp0, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp4, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp1, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.RestartStrip();

                triStream.Append(VertexOutput(wp4,float2(0,0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp1, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp5, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp2, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.RestartStrip();

                triStream.Append(VertexOutput(wp5, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp2, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp3, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.Append(VertexOutput(wp0, float2(0, 0), fixed4(1, 1, 1, 1)));
                triStream.RestartStrip();
                
                
      


                
            }
 
            fixed4 frag (g2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
