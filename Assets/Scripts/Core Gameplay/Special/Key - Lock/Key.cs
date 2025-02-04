using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public static event Action<Key> unlockWithKeyEvent;

    public void Unlock()
    {
        unlockWithKeyEvent?.Invoke(this);
    }
}
