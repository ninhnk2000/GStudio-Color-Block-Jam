using System;
using UnityEngine;

public class ObjectPartHolder : MonoBehaviour
{
    public static event Action<int> dissolveObjectPartEvent;

    private void OnTriggerEnter(Collider other)
    {
        IObjectPart objectPart = other.GetComponent<IObjectPart>();

        if (objectPart != null)
        {
            dissolveObjectPartEvent?.Invoke(other.gameObject.GetInstanceID());
            // other.gameObject.SetActive(false);
        }
    }
}
