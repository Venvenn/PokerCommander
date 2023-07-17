using UnityEngine;
using UnityEditor;
using System;
using System.IO;

/// <summary>
/// A tool used to edit dialogue scripts in engine 
/// </summary>
public class TextEditorTool : EditorWindow
{
    private string m_text;
    private Color m_textColor;
    private string m_rawTextString;
    private string m_scriptedTextString;
    
    private string m_path;
    private bool m_rawText;

    private Vector2 m_horizontalScrollPos;
    private Vector2 m_verticalScrollPos;

    [MenuItem("Window/Text Editor")]
    static void Init()
    {
        GetWindow<TextEditorTool>(false, "Text Editor");
    }
    
    void OnGUI()
    {
        var toolbar_rect = DrawToolbar();
        float y_offset = toolbar_rect.height + toolbar_rect.y;
        var text_rect = new Rect(toolbar_rect.x, y_offset, position.width, position.height - y_offset - 4);

        m_verticalScrollPos = EditorGUILayout.BeginScrollView(m_verticalScrollPos, GUILayout.Height(text_rect.height));
        
        var style = EditorStyles.textArea;
        style.richText = true;
        style.fixedHeight = 0;
        //m_text = GUI.TextArea(text_rect, m_text, style);
        m_text = EditorGUILayout.TextArea(m_text, style, GUILayout.ExpandHeight(true));
        //TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.hotControl);

        EditorGUILayout.EndScrollView();
        
        GUILayout.FlexibleSpace();
    }
    
    Rect DrawToolbar()
    {
        GUIStyle style = new GUIStyle(EditorStyles.toolbar);
        style.fixedHeight = 0;
        
        var rect = EditorGUILayout.BeginHorizontal(style);
        EditorGUI.DrawRect(rect, Color.white * 0.5f);

        Color fallBack = GUI.color;
 
        if (m_rawText)
        {
            GUI.color = Color.green;
            Button(new GUIContent("Raw Text"), FlipRaw, GUILayout.Width(100),GUILayout.Height(32));
        }
        else
        {
            GUI.color = Color.yellow;
            Button(new GUIContent("Scripted Text"), FlipRaw,  GUILayout.Width(100),GUILayout.Height(32));
        }
        GUI.color = fallBack;

        Spacer(5);
        
        Button(new GUIContent("New"), NewFile, GUILayout.Width(48), GUILayout.Height(32));
        Button(new GUIContent("Open"), OpenFile, GUILayout.Width(48), GUILayout.Height(32));
        Button(new GUIContent("Save"), SaveFile, GUILayout.Width(48), GUILayout.Height(32));
        
        Spacer(5);
        
        GUILayout.Label("Colour: ", GUILayout.Width(52), GUILayout.Height(32));
        m_textColor = EditorGUILayout.ColorField(m_textColor, GUILayout.Width(52), GUILayout.Height(32));


        GUIStyle labelStyle = EditorStyles.miniLabel;
        labelStyle.alignment = TextAnchor.MiddleRight;

        EditorGUILayout.LabelField(Path.GetFileName(m_path),labelStyle, GUILayout.ExpandWidth(true));
        
        EditorGUILayout.EndHorizontal();
        
        return rect;
    }

    private void ProcessText()
    {
        //m_text
    }
    
    
    void NewFile()
    {
        m_text = "";
        m_path = "";
        DefocusAndRepaint();
    }

    void OpenFile()
    {
        m_path = EditorUtility.OpenFilePanel("Open text file", "", "*");
        m_text = File.ReadAllText(m_path);
        DefocusAndRepaint();
    }

    void SaveFile()
    {
        if (string.IsNullOrEmpty(m_path))
        {
            m_path = EditorUtility.SaveFilePanel("Save text file", "", "", "*");
        }
        File.WriteAllText(m_path, m_text);
        DefocusAndRepaint();
    }

    void DefocusAndRepaint()
    {
        GUI.FocusControl(null);
        Repaint();
    }
    
    // Function used to draw buttons in one line, Copy it and use it  elsewhere if you want ;)
    void Button(GUIContent content, Action action, params GUILayoutOption[] options)
    {
        GUIStyle style = new GUIStyle(EditorStyles.miniButton);

        style.fixedWidth = 0;
        style.fixedHeight = 0;

        if (GUILayout.Button(content, style, options))
        {
            action();
        }
    }

    private void FlipRaw()
    {
        m_rawText = !m_rawText;
    }
    
    private void Colour()
    {
        
    }

    private void Spacer(int size, bool horizontal = false)
    {
        if (horizontal)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(size));
        }
        else
        {
            EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(size));

        }
    }
    
    
}