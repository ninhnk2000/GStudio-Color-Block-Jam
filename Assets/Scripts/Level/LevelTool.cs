#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LevelTool : EditorWindow
{
    [MenuItem("Color Block Jam - Cloned/LevelTool")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelTool>();
        window.titleContent = new GUIContent("LevelTool");
        window.Show();
    }

    private void OnGUI()
    {

    }
}
#endif
