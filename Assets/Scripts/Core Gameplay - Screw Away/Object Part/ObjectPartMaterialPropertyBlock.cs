using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ObjectPartMaterialPropertyBlock : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private string alphaValueReference;

    #region PRIVATE FIELD
    [SerializeField] private List<Tween> _tweens;
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private Tween _currentAlphaTransition;
    private bool _isDissolving;
    #endregion

    private void Awake()
    {
        BasicObjectPart.selectObjectPartEvent += OnObjectPartSelected;
        BasicObjectPart.deselectObjectPartEvent += OnObjectPartDeselected;
        ObjectPartHolder.dissolveObjectPartEvent += Dissolve;
        BasicObjectPart.dissolveObjectPartEvent += Dissolve;

        _tweens = new List<Tween>();

        Init();
    }

    void OnDestroy()
    {
        BasicObjectPart.selectObjectPartEvent -= OnObjectPartSelected;
        BasicObjectPart.deselectObjectPartEvent -= OnObjectPartDeselected;
        ObjectPartHolder.dissolveObjectPartEvent -= Dissolve;
        BasicObjectPart.dissolveObjectPartEvent -= Dissolve;

        CommonUtil.StopAllTweens(_tweens);
        CommonUtil.StopTween(_currentAlphaTransition);
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

    public void SetAlpha(float alpha)
    {
        Init();

        _propertyBlock.SetFloat(alphaValueReference, alpha);

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    public void OnObjectPartSelected(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            _currentAlphaTransition.Complete();

            _propertyBlock.SetInt("_ZWrite", 0);

            _currentAlphaTransition = Tween.Custom(1, 0.3f, duration: 0.3f, onValueChange: newVal =>
            {
                _propertyBlock.SetFloat(alphaValueReference, newVal);

                _renderer.SetPropertyBlock(_propertyBlock);
            });
        }
    }

    public void OnObjectPartDeselected(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            _currentAlphaTransition.Complete();

            _propertyBlock.SetInt("_ZWrite", 1);

            _currentAlphaTransition = Tween.Custom(0.3f, 1, duration: 0.3f, onValueChange: newVal =>
            {
                _propertyBlock.SetFloat(alphaValueReference, newVal);

                _renderer.SetPropertyBlock(_propertyBlock);
            });
        }
    }

    private void Dissolve(int instanceId)
    {
        if (instanceId != gameObject.GetInstanceID() || _isDissolving)
        {
            return;
        }

        // ParticleSystem fx = ObjectPoolingEverything.GetFromPool<ParticleSystem>(GameConstants.DESTROY_OBJECT_PART_FX);

        // fx.transform.position = transform.position;

        // fx.Play();

        _tweens.Add(Tween.Custom(0f, 1, duration: 0.6f, onValueChange: newVal =>
        {
            _propertyBlock.SetFloat("_DissolveStrength", newVal);

            _renderer.SetPropertyBlock(_propertyBlock);
        })
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        }));

        _isDissolving = true;
    }
}
