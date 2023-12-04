Shader "Custom/VertexColorShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        // Add other properties as needed
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        // Input structure
        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        // Surface shader function
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Modify o.Albedo (color) based on your logic
            o.Albedo = IN.color.rgb;

            // Add other surface shader code as needed
        }
        ENDCG
    }
    FallBack "Diffuse"
}
