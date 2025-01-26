#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SaferioSceneOpener : EditorWindow
{
    [MenuItem("Tools/Open Scene/Menu")]
    public static void OpenMenuScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");
    }

    [MenuItem("Tools/Open Scene/Gameplay")]
    public static void OpenGameplayScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Gameplay - Screw Away.unity");
    }

    [MenuItem("Tools/Open Scene/Level Difficulty Calculator")]
    public static void OpenLevelDifficultyCalculatorScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Level Difficulty Calculator.unity");
    }

    [MenuItem("Tools/Open Persistent Save Path")]
    public static void OpenPersistentSavePath()
    {
        string path = Application.persistentDataPath;

        Process.Start($"\"{path}\"");
    }
}
#endif
