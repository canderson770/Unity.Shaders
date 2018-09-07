using UnityEditor;
using UnityEngine;

public class StandardShaderPlusGUIOLD : ShaderGUI
{
    MaterialEditor materialEditor;
    MaterialProperty[] properties;
    static GUIContent staticLabel = new GUIContent();

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
//        base.OnGUI (materialEditor, properties);
        this.materialEditor = materialEditor;
        this.properties = properties;
        MainMaps();
    }

    private void MainMaps()
    {
        GUILayout.Label("Rendering Mode");

        GUILayout.Label("Main Maps", EditorStyles.boldLabel);

        MaterialProperty mainTex = FindProperty("_MainTex");
        materialEditor.TexturePropertySingleLine(MakeLabel(mainTex, "Albedo (RGB) and Transparency (A)"), mainTex, FindProperty("_Color"));

        MaterialProperty metallicMap = FindProperty("_MetallicGlossMap");
        materialEditor.TexturePropertySingleLine(MakeLabel(metallicMap, "Metallic (R) and Smoothness (A)"), metallicMap, FindProperty("_Metallic"));

        MaterialProperty smoothness = FindProperty("_Glossiness");
        materialEditor.ShaderProperty(smoothness, MakeLabel(smoothness, null, "        "));

        MaterialProperty smoothnessChannel = FindProperty("_SmoothnessTextureChannel");
        materialEditor.ShaderProperty(smoothnessChannel, MakeLabel("Source", null, "           "));

        MaterialProperty normalMap = FindProperty("_BumpMap");
        materialEditor.TexturePropertySingleLine(MakeLabel(normalMap), normalMap, normalMap.textureValue ? FindProperty("_BumpScale") : null);

        MaterialProperty heightMap = FindProperty("_ParallaxMap");
        materialEditor.TexturePropertySingleLine(MakeLabel(heightMap), heightMap, heightMap.textureValue ? FindProperty("_Parallax") : null);

        MaterialProperty occulusionMap = FindProperty("_OcclusionMap");
        materialEditor.TexturePropertySingleLine(MakeLabel(occulusionMap), occulusionMap, occulusionMap.textureValue ? FindProperty("_OcclusionStrength") : null);

        MaterialProperty detailMask = FindProperty("_DetailMask");
        materialEditor.TexturePropertySingleLine(MakeLabel(detailMask), detailMask);

        GUILayout.Label("Emission");
        MaterialProperty emissionMap = FindProperty("_EmissionMap");
//        MaterialProperty emissionColor = FindProperty("_EmissionColor");
//        emissionColor.colorValue = Color.black;
//        emissionColor.colorValue = Color.white;
        materialEditor.TexturePropertySingleLine(MakeLabel("Color"), emissionMap, FindProperty("_EmissionColor"));
        GUILayout.Label("        Global Illumination");

        materialEditor.TextureScaleOffsetProperty(mainTex);



        GUILayout.Label("Secondary Maps", EditorStyles.boldLabel);

        MaterialProperty detailTex = FindProperty("_DetailAlbedoMap");
        materialEditor.TexturePropertySingleLine(MakeLabel(detailTex, "Albedo (RGB) multiplied by 2"), detailTex);

        MaterialProperty detailNormalMap = FindProperty("_DetailNormalMap");
        materialEditor.TexturePropertySingleLine(MakeLabel(detailNormalMap), detailNormalMap, detailNormalMap.textureValue ? FindProperty("_DetailNormalMapScale") : null);

        materialEditor.TextureScaleOffsetProperty(detailTex);

        MaterialProperty uvSet = FindProperty("_UVSec");
        materialEditor.ShaderProperty(uvSet, MakeLabel(uvSet));



        GUILayout.Label("Forward Rendering Options", EditorStyles.boldLabel);

        MaterialProperty specHighlights = FindProperty("_SpecularHighlights");
        materialEditor.ShaderProperty(specHighlights, MakeLabel(specHighlights));

        MaterialProperty reflections = FindProperty("_GlossyReflections");
        materialEditor.ShaderProperty(reflections, MakeLabel(reflections));



        GUILayout.Label("Advanced Options", EditorStyles.boldLabel);
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();


        GUILayout.Label("Custom Options", EditorStyles.boldLabel);

        MaterialProperty cullingMode = FindProperty("_CullMode");
        materialEditor.ShaderProperty(cullingMode, MakeLabel(cullingMode));

        materialEditor.RenderQueueField();
    }

    private MaterialProperty FindProperty(string name)
    {
        return FindProperty(name, properties);
    }

    private static GUIContent MakeLabel(string text, string tooltip = null, string offset = null)
    {
        staticLabel.text = offset + text;
        staticLabel.tooltip = tooltip;
        if (tooltip == null)
            staticLabel.tooltip = text;
        
        return staticLabel;
    }

    private static GUIContent MakeLabel(MaterialProperty property, string tooltip = null, string offset = null)
    {
        staticLabel.text = offset + property.displayName;
        staticLabel.tooltip = tooltip;
        if (tooltip == null)
            staticLabel.tooltip = property.displayName;

        return staticLabel;
    }
}
