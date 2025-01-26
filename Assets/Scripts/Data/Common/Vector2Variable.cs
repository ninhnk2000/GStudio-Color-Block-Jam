using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/ColoringBook/Vector2Variable")]
public class Vector2Variable : ScriptableObject
{
    [SerializeField] private Vector2 value;
    [SerializeField] private bool isSave;
    [SerializeField] private string saveKey;

    public Vector2 Value
    {
        get => value;
        set
        {
            this.value = value;

            // if (isSave)
            // {
            //     Save(saveKey);
            // }
        }
    }

    public void Save(string key)
    {
        DataUtility.SaveWithReferenceLoopHandling(key, value);
    }

    public void Save()
    {
        DataUtility.SaveWithReferenceLoopHandling(saveKey, value);
    }

    public void Load()
    {
        value = DataUtility.Load(saveKey, Vector2.zero);
    }
}
