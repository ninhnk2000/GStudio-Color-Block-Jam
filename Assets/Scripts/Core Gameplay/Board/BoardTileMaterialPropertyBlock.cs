using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BoardTileMaterialPropertyBlock : MonoBehaviour
{
    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private bool _isHighlight;
    #endregion

    private void Awake()
    {
        LevelLoader.updateBoundEvent += SetColorByBoardBounds;
        BaseBlock.disableHighlightTilesEvent += DisableHighlight;

        _tweens = new List<Tween>();

        Init();
    }

    void OnDestroy()
    {
        LevelLoader.updateBoundEvent -= SetColorByBoardBounds;
        BaseBlock.disableHighlightTilesEvent -= DisableHighlight;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void Init()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
    }

    private void DisableHighlight()
    {
        Highlight(false);
    }

    public void Highlight(bool isHighlight)
    {
        if (isHighlight == _isHighlight)
        {
            return;
        }
        else
        {
            _isHighlight = isHighlight;
        }

        CommonUtil.StopAllTweens(_tweens);

        if (isHighlight)
        {
            _tweens.Add(Tween.Custom(new Color(30 / 255f, 38 / 255f, 74 / 255f, 1), Color.white, duration: 0.3f, onValueChange: newVal =>
            {
                _propertyBlock.SetColor("_OutlineColor", newVal);
                _renderer.SetPropertyBlock(_propertyBlock);
            }));
        }
        else
        {
            _tweens.Add(Tween.Custom(Color.white, new Color(30 / 255f, 38 / 255f, 74 / 255f, 1), duration: 0.3f, onValueChange: newVal =>
            {
                _propertyBlock.SetColor("_OutlineColor", newVal);
                _renderer.SetPropertyBlock(_propertyBlock);
            }));
        }
    }

    private void SetColorByBoardBounds(float rightBound, float leftBound, float topBound, float bottomBound)
    {
        // float ratio = 1 - (transform.position.z - bottomBound) / (topBound - bottomBound);

        // Color firstColor = new Color(71 / 255f, 64 / 255f, 207 / 255f, 1);
        // Color secondColor = new Color(65 / 255f, 118 / 255f, 237 / 255f, 1);

        // _propertyBlock.SetColor("_Color", firstColor + ratio * (secondColor - firstColor));

        // _renderer.SetPropertyBlock(_propertyBlock);
    }
}
