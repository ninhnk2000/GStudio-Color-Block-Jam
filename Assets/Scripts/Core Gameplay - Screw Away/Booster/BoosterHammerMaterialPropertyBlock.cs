using PrimeTween;
using UnityEngine;

public class BoosterHammerMaterialPropertyBlock : MonoBehaviour
{
    #region PRIVATE FIELD
    [SerializeField] private Renderer[] renderers;
    private MaterialPropertyBlock _propertyBlock;
    private bool _isDissolving;
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void Dissolve()
    {
        if (_isDissolving)
        {
            return;
        }

        Tween.Custom(0f, 1, startDelay: 0.2f, duration: 0.6f, onValueChange: newVal =>
        {
            _propertyBlock.SetFloat("_DissolveStrength", newVal);

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].SetPropertyBlock(_propertyBlock);
            }
        })
        .OnComplete(() =>
        {
            _propertyBlock.SetFloat("_DissolveStrength", 0);

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].SetPropertyBlock(_propertyBlock);
            }

            gameObject.SetActive(false);

            _isDissolving = false;
        });

        _isDissolving = true;
    }
}
