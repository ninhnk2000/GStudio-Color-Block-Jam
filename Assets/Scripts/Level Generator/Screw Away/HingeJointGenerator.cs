#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static GameEnum;

public class HingeJointGenerator : EditorWindow
{
    private GameObject target;
    private GameObject screwPrefab;
    private Material targetMaterial;



    private Vector3 dimensionSize;
    private Vector3 distance;
    private float expectedScale;
    private float expectedOffsetRatioAutoSnapSurface = 0.55f;
    private Vector3 expectedLocalPositionOffset;
    private Vector3 expectedLocalRotationOffset;



    private float ratioSnapOffset = 0.06f;



    private string prefabPath = "Assets/Prefabs/MyPrefab.prefab";


    private Vector2 scrollPosition;


    [MenuItem("Saferio/Tools/Hinge Joint Generator")]
    public static void ShowWindow()
    {
        GetWindow<HingeJointGenerator>("Hinge Joint Generator");
    }

    private void OnGUI()
    {
        float padding = 20f;

        Rect areaRect = new Rect(padding, padding, position.width - 2 * padding, position.height - 2 * padding);

        GUILayout.BeginArea(areaRect);

        prefabPath = EditorGUILayout.TextField("Prefab Path", prefabPath);

        target = (GameObject)EditorGUILayout.ObjectField("Target", target, typeof(GameObject), true);
        screwPrefab = (GameObject)EditorGUILayout.ObjectField("Screw Prefab", screwPrefab, typeof(GameObject), true);
        targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", targetMaterial, typeof(Material), true);

        dimensionSize = EditorGUILayout.Vector3Field("Dimension Size", dimensionSize);
        distance = EditorGUILayout.Vector3Field("Distance", distance);
        expectedScale = EditorGUILayout.FloatField("Expected Scale", expectedScale);

        expectedLocalPositionOffset = EditorGUILayout.Vector3Field("Expected Local Position Offset", expectedLocalPositionOffset);
        expectedLocalRotationOffset = EditorGUILayout.Vector3Field("Expected Local Rotation Offset", expectedLocalRotationOffset);

        ratioSnapOffset = EditorGUILayout.FloatField("Ratio Snap Offset", ratioSnapOffset);

        expectedOffsetRatioAutoSnapSurface = EditorGUILayout.FloatField("Expected Offset Ratio Auto Snap Surface", expectedOffsetRatioAutoSnapSurface);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

        if (GUILayout.Button("Generate"))
        {
            Generate(prefabPath);
        }

        if (GUILayout.Button("Generate All"))
        {
            GenerateHingeJointAll(prefabPath);
        }

        if (GUILayout.Button("Check HingeJoint Number"))
        {
            CheckHingeJointNumber(prefabPath);
        }

        if (GUILayout.Button("Check HingeJoint Number All Levels"))
        {
            CheckHingeJointNumberAllLevels();
        }

        // if (GUILayout.Button("Reset Local Rotation"))
        // {
        //     ResetLocalRotation(target.transform);
        // }

        // if (GUILayout.Button("Flip Horizontally"))
        // {
        //     FlipHorizontally(Selection.activeTransform, prefabPath);
        // }

        // if (GUILayout.Button("Flip Vertically"))
        // {
        //     FlipVertically(Selection.activeTransform, prefabPath);
        // }

        // if (GUILayout.Button("Move"))
        // {
        //     Move(Selection.activeTransform, prefabPath);
        // }

        // if (GUILayout.Button("Auto Assign Screw Faction"))
        // {
        //     // AutoAssignScrewFaction(prefabPath);
        // }

        if (GUILayout.Button("Spawn Screw"))
        {
            SpawnScrew(prefabPath);
        }

        // if (GUILayout.Button("Assign Material"))
        // {
        //     AssignMaterial(prefabPath);
        // }

        // if (GUILayout.Button("Center Level Pivot"))
        // {
        //     CenterLevelPivot(prefabPath);
        // }

        // if (GUILayout.Button("Remove All Grandchildren Of"))
        // {
        //     RemoveAllGrandchildrenOf();
        // }

        // if (GUILayout.Button("Get Screw Number By Phase"))
        // {
        //     GetScrewNumberByPhase(prefabPath);
        // }

        if (GUILayout.Button("Move All Screws"))
        {
            MoveAllScrews(prefabPath);
        }

        if (GUILayout.Button("Scale All Screws"))
        {
            ScaleAllScrews(prefabPath);
        }

        if (GUILayout.Button("Set Lossy Scale All Screws"))
        {
            SetLossyScaleAllScrews(prefabPath);
        }

        if (GUILayout.Button("Snap Surface"))
        {
            SnapSurface();
        }

        if (GUILayout.Button("Snap Surface All"))
        {
            SnapSurfaceAll();
        }


        if (GUILayout.Button("Snap Rotation Surface"))
        {
            SnapRotationSurface();
        }

        if (GUILayout.Button("Magic Snap"))
        {
            MagicSnap();
        }

        if (GUILayout.Button("Fold All"))
        {
            FoldAllInPrefabMode();
        }

        GUILayout.EndArea();
        EditorGUILayout.EndScrollView();
    }

    private void CenterLevelPivot(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BasicObjectPart[] objectParts = GetComponentsFromAllChildren<BasicObjectPart>(levelPrefab.transform).ToArray();

        Vector3 averagePostition = new Vector3();

        for (int i = 0; i < objectParts.Length; i++)
        {
            averagePostition += objectParts[i].transform.position;
        }

        averagePostition /= objectParts.Length;

        Selection.activeTransform.position = averagePostition;
    }

    private void ResetLocalRotation(Transform target)
    {
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.Euler(Vector3.zero);
        target.localScale = new Vector3(1 / target.transform.parent.localScale.x, 1 / target.transform.parent.localScale.y, 1 / target.transform.parent.localScale.z);
    }

    private void FlipHorizontally(Transform target, string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        Transform flippedObject = Instantiate(target, levelPrefab.transform);

        flippedObject.name = $"{target.name.Replace("- Generated", "")} - Generated";

        Vector3 position = target.transform.position;

        position.y *= -1;

        Vector3 eulerAngle = target.transform.rotation.eulerAngles;

        eulerAngle.y *= -1;

        flippedObject.transform.position = position;
        flippedObject.transform.rotation = Quaternion.Euler(eulerAngle);

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void FlipVertically(Transform target, string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        Transform flippedObject = Instantiate(target, levelPrefab.transform);

        flippedObject.name = $"{target.name.Replace("- Generated", "")} - Generated";

        Vector3 position = target.transform.position;

        position.x *= -1;

        Vector3 eulerAngle = target.transform.rotation.eulerAngles;

        eulerAngle.x *= -1;

        flippedObject.transform.position = position;
        flippedObject.transform.rotation = Quaternion.Euler(eulerAngle);

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void Move(Transform target, string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        Transform clonedObject = Instantiate(target, levelPrefab.transform);

        clonedObject.name = $"{target.name.Replace("- Generated", "")} - Generated";

        clonedObject.transform.position += distance.x * clonedObject.transform.right;
        clonedObject.transform.position += distance.y * clonedObject.transform.up;
        clonedObject.transform.position += distance.z * clonedObject.transform.forward;

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void Generate(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        RemoveOldHingeJoints(target);

        for (int i = 0; i < target.transform.childCount; i++)
        {
            BasicScrew screw = target.transform.GetChild(i).GetComponent<BasicScrew>();

            if (screw != null)
            {
                HingeJoint joint = target.AddComponent<HingeJoint>();

                // WARNING: HingeJoint axis gizmos may not update correctly, so be sure to double-check it.

                joint.connectedBody = screw.GetComponent<Rigidbody>();
                joint.anchor = screw.transform.localPosition;

                // CRUCIAL
                joint.axis = screw.transform.localRotation * Vector3.forward;

                screw.Joint = joint;
            }
        }

        EditorUtility.SetDirty(target);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void RemoveOldHingeJoints(GameObject objectPart)
    {
        // REMOVE OLD HINGE JOINTS
        HingeJoint[] hingeJoints = objectPart.GetComponents<HingeJoint>();

        for (int I = 0; I < hingeJoints.Length; I++)
        {
            DestroyImmediate(hingeJoints[I]);
        }
    }

    private void GenerateHingeJointAll(string path)
    {
        BasicObjectPart[] objectParts = GetComponentsFromAllChildren<BasicObjectPart>(Selection.activeTransform).ToArray();

        for (int i = 0; i < objectParts.Length; i++)
        {
            RemoveOldHingeJoints(objectParts[i].gameObject);

            GenerateHingeJoint(objectParts[i].gameObject);
        }

        // for (int i = 0; i < levelPrefab.transform.childCount; i++)
        // {
        //     GameObject target = levelPrefab.transform.GetChild(i).gameObject;

        //     BasicObjectPart objectPart = target.transform.GetComponent<BasicObjectPart>();

        //     if (objectPart != null)
        //     {
        //         // REMOVE OLD HINGE JOINTS
        //         RemoveOldHingeJoints(objectPart.gameObject);

        //         GenerateHingeJoint(target);
        //     }
        // }

        void GenerateHingeJoint(GameObject target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                BasicScrew screw = target.transform.GetChild(i).GetComponent<BasicScrew>();

                if (screw != null)
                {
                    HingeJoint joint = target.AddComponent<HingeJoint>();

                    // WARNING: HingeJoint axis gizmos may not update correctly, so be sure to double-check it.

                    joint.connectedBody = screw.GetComponent<Rigidbody>();
                    joint.anchor = screw.transform.localPosition;

                    // CRUCIAL
                    joint.axis = screw.transform.localRotation * Vector3.forward;

                    screw.Joint = joint;
                }
            }
        }

        // for (int i = 0; i < target.transform.childCount; i++)
        // {
        //     BasicScrew screw = target.transform.GetChild(i).GetComponent<BasicScrew>();

        //     if (screw != null)
        //     {
        //         HingeJoint joint = target.AddComponent<HingeJoint>();

        //         // WARNING: HingeJoint axis gizmos may not update correctly, so be sure to double-check it.

        //         joint.connectedBody = screw.GetComponent<Rigidbody>();
        //         joint.anchor = screw.transform.localPosition;

        //         // CRUCIAL
        //         joint.axis = screw.transform.localRotation * Vector3.forward;

        //         screw.Joint = joint;
        //     }
        // }

        EditorUtility.SetDirty(Selection.activeGameObject);

        PrefabUtility.SaveAsPrefabAsset(Selection.activeGameObject, path);
    }

    private void CheckHingeJointNumber(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BasicObjectPart[] objectParts = GetComponentsFromAllChildren<BasicObjectPart>(levelPrefab.transform).ToArray();
        BaseScrew[] screw = GetComponentsFromAllChildren<BaseScrew>(levelPrefab.transform).ToArray();

        int numScrew = screw.Length;
        int numHingeJoint = 0;

        for (int i = 0; i < objectParts.Length; i++)
        {
            int numHingeJointInObjectPart = objectParts[i].GetComponents<HingeJoint>().Length;

            numHingeJoint += numHingeJointInObjectPart;
        }

        bool isPassed = numHingeJoint == numScrew;

        if (numScrew % 3 != 0)
        {
            isPassed = false;
        }

        string resultText = $"<color=#89FF5B>Passed</color>";

        if (!isPassed)
        {
            resultText = $"<color=#FF7878>Failed</color>";
        }

        Debug.Log($"{levelPrefab.name}: {numHingeJoint}/{numScrew} - {resultText}<color=#fff></color>");
    }

    private void CheckHingeJointNumberAllLevels()
    {
        for (int i = 1; i <= 50; i++)
        {
            CheckHingeJointNumber($"Assets/Prefabs/Screw Away/Levels/Level {i}.prefab");
        }
    }

    private void AutoAssignScrewFaction(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BaseScrew[] screws = GetComponentsFromAllChildren<BaseScrew>(levelPrefab.transform).ToArray();

        int currentFaction = 0;

        List<GameFaction> remainingFactionForScrews = new List<GameFaction>();

        for (int i = 0; i < screws.Length; i++)
        {
            remainingFactionForScrews.Add(GameConstants.SCREW_FACTION[currentFaction]);

            if (i > 0 && (i + 1) % 3 == 0)
            {
                currentFaction++;

                if (currentFaction >= GameConstants.SCREW_FACTION.Length)
                {
                    currentFaction = 0;
                }
            }
        }

        for (int i = 0; i < screws.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, remainingFactionForScrews.Count);

            screws[i].ScrewId = i;
            screws[i].Faction = remainingFactionForScrews[randomIndex];

            remainingFactionForScrews.RemoveAt(randomIndex);
        }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void SpawnScrew(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        Vector3 position = new Vector3();

        int currentX = 0;
        int currentY = 0;
        int currentZ = 0;

        dimensionSize.x = Mathf.Max(dimensionSize.x, 1);
        dimensionSize.y = Mathf.Max(dimensionSize.y, 1);
        dimensionSize.z = Mathf.Max(dimensionSize.z, 1);

        Transform target = Selection.activeTransform;

        for (int i = 0; i < dimensionSize.x * dimensionSize.y * dimensionSize.z; i++)
        {
            GameObject screw = (GameObject)PrefabUtility.InstantiatePrefab(screwPrefab, target.transform);

            position.x = (-(dimensionSize.x - 1) / 2f + currentX) * distance.x;
            position.y = (-(dimensionSize.y - 1) / 2f + currentY) * distance.y;
            position.z = (-(dimensionSize.z - 1) / 2f + currentZ) * distance.z;

            currentX++;

            if (currentX >= dimensionSize.x)
            {
                currentY++;

                if (currentY >= dimensionSize.y)
                {
                    currentZ++;

                    currentY = 0;
                }

                currentX = 0;
            }

            // for (int j = 0; j < dimensionSize.x; j++)
            // {
            //     position.x = (-(dimensionSize.x - 1) / 2f + currentX) * distance.x + target.transform.position.x;
            // }

            // for (int j = 0; j < dimensionSize.y; j++)
            // {
            //     position.y = (-(dimensionSize.y - 1) / 2f + currentY) * distance.y + target.transform.position.y;
            // }

            // for (int j = 0; j < dimensionSize.z; j++)
            // {
            //     position.z = (-(dimensionSize.z - 1) / 2f + currentZ) * distance.z + target.transform.position.z;
            // }

            screw.transform.localPosition = position + expectedLocalPositionOffset;
            screw.transform.localEulerAngles += expectedLocalRotationOffset;
            screw.transform.localScale = TransformUtil.ComponentWiseDivine(expectedScale * Vector3.one, target.transform.localScale);
        }




        // for (int i = 0; i < dimensionSize.x; i++)
        // {
        //     for (int j = 0; j < dimensionSize.y; j++)
        //     {
        //         GameObject screw = (GameObject)PrefabUtility.InstantiatePrefab(screwPrefab, target.transform);

        //         Vector3 position;

        //         position.x = (-(dimensionSize.x - 1) / 2f + j) * distance.x;
        //         position.y = (-(dimensionSize.y - 1) / 2f + i) * distance.y;
        //         position.z = (-(dimensionSize.y - 1) / 2f + i) * distance.z;

        //         position += target.transform.position;

        //         screw.transform.position = position;
        //         screw.transform.localScale = TransformUtil.ComponentWiseDivine(expectedScale * Vector3.one, target.transform.localScale);
        //     }
        // }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void AssignMaterial(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BasicObjectPart[] objectParts = GetComponentsFromAllChildren<BasicObjectPart>(levelPrefab.transform).ToArray();

        for (int i = 0; i < objectParts.Length; i++)
        {
            MeshRenderer meshRenderer = objectParts[i].GetComponent<MeshRenderer>();

            Debug.Log(meshRenderer);

            List<Material> materials = meshRenderer.materials.ToList();

            materials[0] = targetMaterial;

            meshRenderer.SetMaterials(materials);
        }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    #region MOVE
    private void MoveAllScrews(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BaseScrew[] screws = GetComponentsFromAllChildren<BaseScrew>(levelPrefab.transform).ToArray();

        for (int i = 0; i < screws.Length; i++)
        {
            if (!screws[i].gameObject.activeSelf)
            {
                continue;
            }

            screws[i].transform.position -= ratioSnapOffset * screws[i].transform.lossyScale.x * screws[i].transform.forward;
        }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void ScaleAllScrews(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BaseScrew[] screws = GetComponentsFromAllChildren<BaseScrew>(levelPrefab.transform).ToArray();

        if (expectedScale <= 0)
        {
            expectedScale = 1;
        }

        for (int i = 0; i < screws.Length; i++)
        {
            screws[i].transform.localScale *= expectedScale;
        }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }

    private void SetLossyScaleAllScrews(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        BaseScrew[] screws = GetComponentsFromAllChildren<BaseScrew>(levelPrefab.transform).ToArray();

        if (expectedScale <= 0)
        {
            expectedScale = 1;
        }

        for (int i = 0; i < screws.Length; i++)
        {
            screws[i].transform.localScale = TransformUtil.ComponentWiseDivine(expectedScale * screws[i].transform.localScale, screws[i].transform.lossyScale);
        }

        EditorUtility.SetDirty(levelPrefab);

        PrefabUtility.SaveAsPrefabAsset(levelPrefab, path);
    }
    #endregion

    #region REMOVE
    private void RemoveAllGrandchildrenOf()
    {
        List<GameObject> toRemoveList = new List<GameObject>();

        for (int i = 0; i < Selection.activeTransform.childCount; i++)
        {
            for (int j = 0; j < Selection.activeTransform.GetChild(i).childCount; j++)
            {
                toRemoveList.Add(Selection.activeTransform.GetChild(i).GetChild(j).gameObject);
            }
        }

        foreach (var item in toRemoveList)
        {
            DestroyImmediate(item);
        }

        EditorUtility.SetDirty(Selection.activeTransform);
    }
    #endregion

    #region INFO
    private void GetScrewNumberByPhase(string path)
    {
        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(path);

        MultiPhaseScrew[] screws = GetComponentsFromAllChildren<MultiPhaseScrew>(levelPrefab.transform).ToArray();

        Dictionary<int, int> numberScrewByPhase = new Dictionary<int, int>();

        for (int i = 0; i < screws.Length; i++)
        {
            if (numberScrewByPhase.ContainsKey(screws[i].Phase))
            {
                numberScrewByPhase[screws[i].Phase]++;
            }
            else
            {
                numberScrewByPhase.Add(screws[i].Phase, 1);
            }
        }

        foreach (var phase in numberScrewByPhase.Keys)
        {
            Debug.Log($"Phase {phase}: {numberScrewByPhase[phase]}");
        }
    }
    #endregion

    #region SNAP
    private void SnapSurface(Transform target)
    {
        Transform selectedObject = target;

        Vector3 screwSize = selectedObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        Vector3 origin = selectedObject.position + 0.8f * selectedObject.lossyScale.z * screwSize.z * selectedObject.forward;

        RaycastHit hit;

        if (selectedObject.gameObject.scene.GetPhysicsScene().Raycast(origin, -selectedObject.forward, out hit, 10))
        {
            IObjectPart objectPart = hit.collider.GetComponent<IObjectPart>();

            if (objectPart != null)
            {
                selectedObject.position = hit.point - expectedOffsetRatioAutoSnapSurface * selectedObject.lossyScale.z * screwSize.z * selectedObject.forward;

                Quaternion surfaceRotation = Quaternion.FromToRotation(selectedObject.forward, hit.normal) * selectedObject.rotation;

                selectedObject.rotation = surfaceRotation;
            }
        }
    }

    private void SnapSurface()
    {
        SnapSurface(Selection.activeTransform);
    }

    private void SnapSurfaceAll()
    {
        BaseScrew[] screws = GetComponentsFromAllChildren<BaseScrew>(Selection.activeTransform).ToArray();

        foreach (var screw in screws)
        {
            SnapSurface(screw.transform);
        }

        EditorUtility.SetDirty(Selection.activeTransform);
    }

    private void SnapParentSurface()
    {
        Transform selectedObject = Selection.activeTransform;

        Vector3 screwSize = selectedObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        Vector3 origin = selectedObject.position + 1f * selectedObject.lossyScale.z * screwSize.z * selectedObject.forward;
        Vector3 direction = selectedObject.parent.position - selectedObject.position;

        RaycastHit hit;

        if (selectedObject.gameObject.scene.GetPhysicsScene().Raycast(origin, direction, out hit, 10))
        {
            IObjectPart objectPart = hit.collider.GetComponent<IObjectPart>();

            if (objectPart != null)
            {
                selectedObject.position = hit.point - 0.3f * selectedObject.lossyScale.z * screwSize.z * selectedObject.forward;

                Quaternion surfaceRotation = Quaternion.FromToRotation(selectedObject.forward, hit.normal) * selectedObject.rotation;

                selectedObject.rotation = surfaceRotation;
            }
        }
    }

    private void SnapRotationSurface()
    {
        Transform selectedObject = Selection.activeTransform;

        RaycastHit hit;

        Vector3 direction = selectedObject.parent.position - selectedObject.position;

        Vector3 screwSize = selectedObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        Vector3 origin = selectedObject.position + 0.8f * selectedObject.lossyScale.z * screwSize.z * selectedObject.forward;

        if (selectedObject.gameObject.scene.GetPhysicsScene().Raycast(origin, direction, out hit, 10))
        {
            IObjectPart objectPart = hit.collider.GetComponent<IObjectPart>();

            if (objectPart != null)
            {
                Quaternion surfaceRotation = Quaternion.FromToRotation(selectedObject.forward, hit.normal) * selectedObject.rotation;

                selectedObject.rotation = surfaceRotation;
            }
        }
    }

    private void MagicSnap()
    {
        SnapRotationSurface();
        SnapSurface();
    }
    #endregion

    #region UTIL
    private void FoldAllInPrefabMode()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            FoldRecursively(prefabStage.prefabContentsRoot.transform);
        }
        else
        {
            Debug.LogWarning("Not in Prefab Mode!");
        }
    }

    private static void FoldRecursively(Transform parentTransform)
    {
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var window = GetWindow(type);
        var exprec = type.GetMethod("SetExpandedRecursive");
        exprec.Invoke(window, new object[] { Selection.activeGameObject.GetInstanceID(), false });

        foreach (Transform child in parentTransform)
        {
            FoldRecursively(child);
        }
    }

    private static EditorWindow GetHierarchyWindow()
    {
        var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var window in windows)
        {
            if (window.titleContent.text == "Hierarchy")
            {
                return window;
            }
        }
        return null;
    }


    public List<T> GetComponentsFromAllChildren<T>(Transform parent) where T : Component
    {
        List<T> components = new List<T>();
        GetComponentsFromAllChildrenRecursive<T>(parent, components);
        return components;
    }

    private void GetComponentsFromAllChildrenRecursive<T>(Transform parent, List<T> components) where T : Component
    {
        T component = parent.GetComponent<T>();
        if (component != null)
        {
            components.Add(component);
        }

        foreach (Transform child in parent)
        {
            GetComponentsFromAllChildrenRecursive<T>(child, components);
        }
    }
    #endregion
}
#endif
