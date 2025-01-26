using PrimeTween;
using UnityEngine;

public class ScrewOutline : MonoBehaviour
{
    [SerializeField] private ScrewServiceLocator screwServiceLocator;

    [SerializeField] private float outlineWidth;

    private Tween _outlineTween;
    private bool _isOutlined;
    private bool _isScrewLoosed;

    private void Awake()
    {
        BaseScrew.screwStartLoosenedEvent += OnScrewStartLoosed;
    }

    void OnDestroy()
    {
        BaseScrew.screwStartLoosenedEvent -= OnScrewStartLoosed;

        CommonUtil.StopTween(_outlineTween);
    }

    public void ShowOutline()
    {
        if (_isScrewLoosed)
        {
            return;
        }

        CommonUtil.StopTween(_outlineTween);

        _outlineTween = Tween.Custom(0.5f, outlineWidth, duration: 0.3f, onValueChange: newVal =>
        {
            screwServiceLocator.screwMaterialPropertyBlock.SetOutlineWidth(newVal);

            _isOutlined = true;
        });
    }

    public void HideOutline()
    {
        CommonUtil.StopTween(_outlineTween);

        _outlineTween = Tween.Custom(outlineWidth, 0.5f, duration: 0.3f, onValueChange: newVal =>
        {
            screwServiceLocator.screwMaterialPropertyBlock.SetOutlineWidth(newVal);

            _isOutlined = false;
        });
    }

    private void OnScrewStartLoosed(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            if (_isOutlined)
            {
                HideOutline();
            }

            _isScrewLoosed = true;
        }
    }
}
