using UnityEngine;

public class BoosterHammer : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitFx;

    [SerializeField] private BoosterHammerMaterialPropertyBlock boosterHammerMaterialPropertyBlock;

    public void PlayHitFx()
    {
        hitFx.gameObject.SetActive(true);

        hitFx.Play();
    }

    public void Dissolve()
    {
        boosterHammerMaterialPropertyBlock.Dissolve();
    }
}
