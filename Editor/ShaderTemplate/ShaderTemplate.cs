using UnityEngine;
using UnityEditor;

public class ShaderTemplate
{
    [MenuItem("Assets/Create/Shader/URP_Blank", false, 85)]
    public static void CreateCustomShader()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
            path = "Assets";
        else if (System.IO.Path.GetExtension(path) != "")
            path = path.Replace(System.IO.Path.GetFileName(path), "");

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/NewCustomShader.shader");
        string shaderName = System.IO.Path.GetFileNameWithoutExtension(assetPathAndName);
        string template = $@"
Shader ""Custom/{shaderName}""
{{
    Properties
    {{ 
        // Add properties here
    }}

    SubShader
    {{
        Tags {{ ""RenderType"" = ""Opaque"" ""RenderPipeline"" = ""UniversalRenderPipeline"" }}

        Pass
        {{
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include ""Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl""

            struct Attributes
            {{
                float4 positionOS   : POSITION;                 
            }};

            struct Varyings
            {{
                float4 positionHCS  : SV_POSITION;
            }};            

            Varyings vert(Attributes IN)
            {{
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                
                return OUT;
            }}

            half4 frag() : SV_Target
            {{
                half4 customColor;
                customColor = half4(0.5, 0, 0, 1); // Example color
                return customColor;
            }}
            ENDHLSL
        }}
    }}
}}";

        
        ProjectWindowUtil.CreateAssetWithContent(assetPathAndName, template);
    }
}
