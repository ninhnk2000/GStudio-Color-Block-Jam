#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static GameEnum;

public class SaferioMeshExporter : EditorWindow
{
    private MeshCollider targetMeshCollider;

    [MenuItem("Saferio/Tools/Saferio Mesh Exporter")]
    public static void ShowWindow()
    {
        GetWindow<SaferioMeshExporter>("Saferio Mesh Exporter");
    }

    private void OnGUI()
    {
        float padding = 20f;

        Rect areaRect = new Rect(padding, padding, position.width - 2 * padding, position.height - 2 * padding);

        GUILayout.BeginArea(areaRect);

        targetMeshCollider =
            (MeshCollider)EditorGUILayout.ObjectField("Target Mesh Collider", targetMeshCollider, typeof(MeshCollider), true);

        if (GUILayout.Button("Extract"))
        {
            Extract();
        }

        GUILayout.EndArea();
    }

    private void Extract()
    {
        AssetDatabase.CreateAsset(targetMeshCollider.sharedMesh, $"Assets/Models/Screw Away/Generated Convex Mesh/{targetMeshCollider.name}.asset");
    }
}
#endif
