using UnityEditor;
using UnityEngine;

public class SEMCameraShaderGUI : ShaderGUI
{
    MaterialEditor m_editor;
    MaterialProperty[] m_props;
    static GUIContent staticLabel = new GUIContent();

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        m_editor = materialEditor;
        m_props = props;

        ShaderPropertiesGUI();
    }

    private void ShaderPropertiesGUI()
    {
        MaterialProperty mainTex = FindProperty("_MainTex");
        MaterialProperty colorTint = FindProperty("_Color");

        if (mainTex != null && colorTint != null)
            m_editor.TexturePropertySingleLine(MakeLabel(mainTex, "Render Texture for SEM Camera"), mainTex, colorTint);

        MaterialProperty grayscale = FindProperty("_GrayScaleOn");
        if (grayscale != null)
            m_editor.ShaderProperty(grayscale, MakeLabel(grayscale, "Make SEM Render Texture grayscale"));


        AddSpace(1);


        MaterialProperty noiseMap = FindProperty("_NoiseTex");
        if (noiseMap != null)
            m_editor.TexturePropertySingleLine(MakeLabel(noiseMap), noiseMap);

        MaterialProperty movement = FindProperty("_Speed");
        if (movement != null)
            m_editor.ShaderProperty(movement, MakeLabel(movement, "Should noise move?"));



        MaterialProperty scanlinePos = FindProperty("_ScanPos");
        if (scanlinePos != null)
            m_editor.ShaderProperty(scanlinePos, MakeLabel(scanlinePos, "World Y Position of scan line"));



        MaterialProperty topClarity = FindProperty("_TopClarity");
        if (topClarity != null)
            m_editor.ShaderProperty(topClarity, MakeLabel(topClarity, "Clarity of screen above scan line"));

        MaterialProperty bottomClarity = FindProperty("_BottomClarity");
        if (bottomClarity != null)
            m_editor.ShaderProperty(bottomClarity, MakeLabel(bottomClarity, "Clarity of screen below scan line"));



        GUILayout.Label("", EditorStyles.boldLabel);

        m_editor.RenderQueueField();
        m_editor.EnableInstancingField();
        m_editor.DoubleSidedGIField();
    }

    private MaterialProperty FindProperty(string name)
    {
        return FindProperty(name, m_props);
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

    private void AddSpace(int amountOfSpaces)
    {
        for (int i = 0; i < amountOfSpaces; i++)
        {
            EditorGUILayout.Space();
        }
    }
}
