using System;
using System.Collections;
using Dreamteck.Splines;
using Lean.Localization;
using PrimeTween;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorialHand;
    [SerializeField] private SpriteRenderer tutorialHandRenderer;
    [SerializeField] private BlockMaterialPropertyBlock blockMaterialPropertyBlock;

    [SerializeField] private TMP_Text tutorialText;

    #region PRIVATE FIELD
    private int _step;
    private Vector2 _screenSizeWorld;
    #endregion

    #region EVENT
    public static event Action<bool> enableSwipingEvent;
    public static event Action<bool> enablePinchingEvent;
    #endregion

    void Awake()
    {
        BaseBlock.blockCompletedEvent += OnBlockCompleted;

        StartCoroutine(TutorialStepOne());

        _screenSizeWorld = CommonUtil.GetScreenSizeWorld(Camera.main);
    }

    void OnDestroy()
    {
        BaseBlock.blockCompletedEvent -= OnBlockCompleted;
    }

    private IEnumerator TutorialStepOne()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        int phase = 0;

        while (_step == 0)
        {
            if (phase % 3 == 0)
            {
                blockMaterialPropertyBlock.ShowOutline(true);
            }
            else if (phase % 3 == 1)
            {
                blockMaterialPropertyBlock.ShowOutline(false);
            }
            else if (phase % 3 == 2)
            {
                Tween.PositionZ(tutorialHand, tutorialHand.position.z + 6, duration: 0.3f, cycles: 2, cycleMode: CycleMode.Yoyo);
            }

            phase++;

            yield return waitForSeconds;
        }

        EndTutorial();
    }

    private void EndTutorial()
    {
        // Tween.Custom(1, 0, duration: 0.3f, onValueChange: newVal =>
        // {

        // });

        Tween.Alpha(tutorialText, 0, duration: 0.3f);
        Tween.Alpha(tutorialHandRenderer, 0, duration: 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    #region CALLBACK
    private void OnBlockCompleted()
    {
        if (_step == 0)
        {
            _step++;
        }
    }
    #endregion
}
