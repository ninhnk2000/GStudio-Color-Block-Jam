using System;
using System.Collections;
using Dreamteck.Splines;
using Lean.Localization;
using PrimeTween;
using Saferio.Util.SaferioTween;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorialHand;
    [SerializeField] private SpriteRenderer ripple;
    [SerializeField] private SpriteRenderer tutorialHandRenderer;
    [SerializeField] private BlockMaterialPropertyBlock blockMaterialPropertyBlock;

    [SerializeField] private TMP_Text tutorialText;

    #region PRIVATE FIELD
    private int _step;
    private Vector2 _screenSizeWorld;
    private float _initialTutorialHandPosition;
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
        _initialTutorialHandPosition = tutorialHand.position.z;
    }

    void OnDestroy()
    {
        BaseBlock.blockCompletedEvent -= OnBlockCompleted;
    }

    private IEnumerator TutorialStepOne()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);

        int phase = 0;

        ripple.color = ColorUtil.WithAlpha(ripple.color, 0);
        ripple.gameObject.SetActive(false);

        while (_step == 0)
        {
            if (phase == 0)
            {
                blockMaterialPropertyBlock.ShowOutline(true);

                // Tween.Alpha(ripple, 1, duration: 0.3f);
            }
            else if (phase == 1)
            {
                blockMaterialPropertyBlock.ShowOutline(false);

                // Tween.Alpha(ripple, 0, duration: 0.3f);
            }
            else if (phase == 3)
            {
                Tween.PositionZ(tutorialHand, tutorialHand.position.z + 12, duration: 0.5f)
                .OnComplete(() =>
                {
                    Tween.Alpha(tutorialHandRenderer, 0, duration: 0.2f)
                    .OnComplete(() =>
                    {
                        tutorialHand.position = tutorialHand.position.ChangeZ(_initialTutorialHandPosition);

                        Tween.Alpha(tutorialHandRenderer, 1, duration: 0.2f);
                    });
                }
                );
            }

            phase++;

            if (phase >= 4)
            {
                phase = 0;
            }

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
