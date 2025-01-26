using UnityEngine;

public interface ISaferioScrollViewItem
{
    public void Setup(int index, RectTransform parent);
    public bool IsValidAtIndex(int index);
    public void Refresh(int index);
}
