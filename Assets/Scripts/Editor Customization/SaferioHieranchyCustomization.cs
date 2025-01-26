#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]  // Makes sure the editor script is initialized when Unity starts.
public static class SaferioHierarchyCustomization
{
    // static SaferioHierarchyCustomization()
    // {
    //     // Hook into the GUI callback for the hierarchy window
    //     EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    // }

    // private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    // {
    //     GameObject go = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

    //     // Make sure we only affect GameObjects (not other items)
    //     if (go != null)
    //     {
    //         // Define your custom condition for the name color change
    //         if (go.name.Contains("Special"))
    //         {
    //             // Set the color to red if the name contains "Special"
    //             GUI.color = Color.red;
    //         }
    //         else
    //         {
    //             // Set the color back to the default white
    //             GUI.color = Color.white;
    //         }

    //         // Draw the GameObject name with the set color
    //         // EditorGUI.LabelField(selectionRect, go.name);
    //     }
    // }
}
#endif
