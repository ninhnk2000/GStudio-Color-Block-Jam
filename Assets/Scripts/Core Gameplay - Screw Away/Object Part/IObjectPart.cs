using UnityEngine;

public interface IObjectPart
{
    public void Select();
    public void Break(Vector3 touchPosition);
    public bool IsConcave
    {
        get;
    }
}
