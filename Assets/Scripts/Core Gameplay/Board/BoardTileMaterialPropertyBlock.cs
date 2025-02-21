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
    #endregion

    private void Awake()
    {
        LevelLoader.updateBoundEvent += SetColorByBoardBounds;

        _tweens = new List<Tween>();

        Init();
    }

    void OnDestroy()
    {
        LevelLoader.updateBoundEvent -= SetColorByBoardBounds;

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

    private void SetColorByBoardBounds(float rightBound, float leftBound, float topBound, float bottomBound)
    {
        // float ratio = 1 - (transform.position.z - bottomBound) / (topBound - bottomBound);

        // Color firstColor = new Color(71 / 255f, 64 / 255f, 207 / 255f, 1);
        // Color secondColor = new Color(65 / 255f, 118 / 255f, 237 / 255f, 1);

        // _propertyBlock.SetColor("_Color", firstColor + ratio * (secondColor - firstColor));

        // _renderer.SetPropertyBlock(_propertyBlock);
    }
}
