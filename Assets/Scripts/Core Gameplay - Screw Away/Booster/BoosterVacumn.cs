using System;
using PrimeTween;
using UnityEngine;

public class BoosterVacumn : MonoBehaviour
{
    [SerializeField] private BoosterHammerMaterialPropertyBlock boosterVacumnMaterialPropertyBlock;

    public void Vacumn(Action<Vector3> onCompletedAction)
    {
        gameObject.SetActive(true);

        transform.position = new Vector3(0, 8, -20);

        Tween.PositionZ(transform, -10, duration: 0.5f).OnComplete(() =>
        {
            Tween.Delay(0.3f).OnComplete(() => Dissolve());

            onCompletedAction?.Invoke(transform.position);
        });
    }

    public void Dissolve()
    {
        boosterVacumnMaterialPropertyBlock.Dissolve();
    }
}
