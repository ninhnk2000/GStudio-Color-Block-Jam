using UnityEngine;
using static GameEnum;

public class ScrewMaterialPropertyBlock : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Material[] materials;

    [Header("CUSTOMIZE")]
    [SerializeField] private string colorReference;
    [SerializeField] private string secondaryColorReference;
    [SerializeField] private string outlineWidthReference;
    [SerializeField] private float secondaryColorMultiplier;

    #region PRIVATE FIELD
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    #endregion

    private void Awake()
    {
        Init();
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

    public void SetColor(GameFaction faction)
    {
        meshRenderer.material = materials[(int)faction];

        meshRenderer.material.SetColor("_SecondaryColor", meshRenderer.material.GetColor("_Color") * 0.4f);

        return;
    }

    public void SetColor(Color color)
    {
        meshRenderer.material = materials[Random.Range(0, materials.Length)];

        return;

        Init();

        _propertyBlock.SetColor(colorReference, color);
        _propertyBlock.SetColor(secondaryColorReference, secondaryColorMultiplier * color);

        _renderer.SetPropertyBlock(_propertyBlock);
        // _screwInsideRenderer.SetPropertyBlock(_propertyBlock);
    }

    public void SetOutlineWidth(float value)
    {
        _propertyBlock.SetFloat(outlineWidthReference, value);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
