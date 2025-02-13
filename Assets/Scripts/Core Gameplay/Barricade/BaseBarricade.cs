using UnityEngine;
using static GameEnum;

public class BaseBarricade : MonoBehaviour
{
    [SerializeField] private Transform blockSmasher;

    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] private BarricadeProperty barricadeProperty;
    [SerializeField] private float disintegrationDuration;

    #region PRIVATE FIELD
    private MeshRenderer _blockSmasherRenderer;
    #endregion

    public BarricadeServiceLocator BarricadeServiceLocator
    {
        get => barricadeServiceLocator;
    }

    public BarricadeProperty BarricadeProperty
    {
        get => barricadeProperty;
    }

    public Direction Direction
    {
        get => barricadeProperty.Direction;
        set => barricadeProperty.Direction = value;
    }

    public GameFaction Faction
    {
        get => barricadeProperty.Faction;
    }

    public float DisintegrationDuration
    {
        get => disintegrationDuration;
    }

    public MeshRenderer BlockSmasherRenderer
    {
        get => _blockSmasherRenderer;
    }

    private void Awake()
    {
        _blockSmasherRenderer = blockSmasher.GetComponent<MeshRenderer>();

        barricadeServiceLocator.barricadeFaction.SetFaction(barricadeProperty.Faction);
    }

    void Update()
    {
        blockSmasher.transform.RotateAround(blockSmasher.transform.right, 0.03f);
        // blockSmasher.transform.eulerAngles += new Vector3(1, 0, 0);
    }

    private void OnValidate()
    {
        _blockSmasherRenderer = blockSmasher.GetComponent<MeshRenderer>();

        barricadeServiceLocator.barricadeFaction.SetFaction(barricadeProperty.Faction);
    }
}
