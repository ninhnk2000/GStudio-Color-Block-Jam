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
    [SerializeField] private SpriteRenderer tutorialHandSprite;
    [SerializeField] private ScrewOutline screwOutline;
    [SerializeField] private Transform screw;
    [SerializeField] private Transform spline;
    [SerializeField] private SplinePositioner tutorialHandSplinePositioner;
    [SerializeField] private SplineFollower tutorialHandSplineFollower;
    [SerializeField] private SkeletonGraphic pinchHand;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text selectTutorialText;
    [SerializeField] private TMP_Text scaleTutorialText;
    [SerializeField] private TMP_Text pinchTutorialText;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedTutorialText;

    [SerializeField] private SpriteRenderer rotateIconWorld;
    // [SerializeField] private Image rotateIcon;
    [SerializeField] private Image[] scaleIcons;
    [SerializeField] private SpriteRenderer scaleIconHand;

    [SerializeField] private string[] translationNames;
    [SerializeField] private Vector3 tutorialHandEulerAngles;
    [SerializeField] private Vector3 pinchTutorialHandPosition;

    #region PRIVATE FIELD
    private int _step;
    private Vector2 _screenSizeWorld;
    private bool _isConstraintHand;
    private Vector3 _initialRotateIconPosition;
    private Vector3 _initialRotateIconRotation;
    #endregion

    #region EVENT
    public static event Action<bool> enableSwipingEvent;
    public static event Action<bool> enablePinchingEvent;
    #endregion

    void Awake()
    {
        ScrewSelectionInput.selectScrewEvent += OnScrewSelected;
        ScrewSelectionInput.breakObjectEvent += OnScrewSelected;
        SwipeGesture.swipeGestureEvent += OnScreenSwiped;
        PinchGesture.pinchGestureEvent += OnScreenZoomed;

        StartCoroutine(TutorialStepOne());

        _screenSizeWorld = CommonUtil.GetScreenSizeWorld(Camera.main);

        rotateIconWorld.gameObject.SetActive(false);
        scaleIcons[0].gameObject.SetActive(false);
        scaleIcons[1].gameObject.SetActive(false);
        scaleIconHand.gameObject.SetActive(false);

        pinchHand.gameObject.SetActive(false);

        _initialRotateIconPosition = rotateIconWorld.transform.position;
        _initialRotateIconRotation = rotateIconWorld.transform.eulerAngles;
    }

    private void Start()
    {
        enableSwipingEvent?.Invoke(false);
        enablePinchingEvent?.Invoke(false);
    }

    void Update()
    {
        tutorialHand.rotation = Quaternion.Euler(tutorialHandEulerAngles);
        spline.rotation = Quaternion.Euler(new Vector3(30, 0, 0));

        rotateIconWorld.transform.position = _initialRotateIconPosition;
        rotateIconWorld.transform.rotation = Quaternion.Euler(_initialRotateIconRotation);

        if (_isConstraintHand)
        {
            tutorialHand.position = pinchTutorialHandPosition;
        }
    }

    void OnDestroy()
    {
        ScrewSelectionInput.selectScrewEvent -= OnScrewSelected;
        ScrewSelectionInput.breakObjectEvent -= OnScrewSelected;
        SwipeGesture.swipeGestureEvent -= OnScreenSwiped;
        PinchGesture.pinchGestureEvent -= OnScreenZoomed;
    }

    private IEnumerator TutorialStepOne()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(Time.deltaTime);

        Vector3 targetPosition;

        targetPosition = screw.position + 0.8f * screw.right;
        targetPosition.y += 0.8f;

        tutorialHand.position = targetPosition;

        float lerpDelta = 0.05f;
        float lerpProgress = 0;

        int phase = 0;

        bool isScrewOulined = false;

        while (_step == 0)
        {
            if (phase % 2 == 0)
            {
                if (!isScrewOulined)
                {
                    screwOutline.ShowOutline();

                    isScrewOulined = true;
                }

                targetPosition = screw.position + 0.8f * screw.right;
                targetPosition.y += 1f;
            }
            else
            {
                if (isScrewOulined)
                {
                    screwOutline.HideOutline();

                    isScrewOulined = false;
                }

                targetPosition = screw.position - 3 * screw.right;
                targetPosition.y -= 3;
            }

            targetPosition.y += 0.08f * _screenSizeWorld.y;
            targetPosition.z = -5;

            tutorialHand.position = Vector3.Lerp(tutorialHand.position, targetPosition, lerpDelta);

            lerpProgress += lerpDelta;

            if (lerpProgress >= 1)
            {
                phase++;
                lerpProgress = 0;
            }

            yield return waitForSeconds;
        }

        screwOutline.HideOutline();

        StartCoroutine(TutorialStepTwo());
    }

    private IEnumerator TutorialStepTwo()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(Time.deltaTime);

        float lerpDelta = 1f / 3;
        float lerpProgress = 0;

        int phase = 0;

        // localizedTutorialText.TranslationName = translationNames[1];
        selectTutorialText.gameObject.SetActive(false);
        scaleTutorialText.gameObject.SetActive(true);

        rotateIconWorld.gameObject.SetActive(true);
        rotateIconWorld.color = ColorUtil.WithAlpha(rotateIconWorld.color, 0);

        Tween.Alpha(rotateIconWorld, 1, duration: 0.3f);

        // tutorialHandSplinePositioner.enabled = true;

        float percent = 0;

        enableSwipingEvent?.Invoke(true);

        while (_step == 1)
        {
            // tutorialHand.rotation = Quaternion.Euler(tutorialHandEulerAngles);
            if (phase < 2)
            {
                if (phase % 2 == 0)
                {
                    tutorialHandSprite.color = Color.Lerp(tutorialHandSprite.color, ColorUtil.WithAlpha(tutorialHandSprite.color, 0), lerpDelta);
                }
                else
                {
                    tutorialHandSprite.color = Color.Lerp(tutorialHandSprite.color, ColorUtil.WithAlpha(tutorialHandSprite.color, 1), lerpDelta);

                    // percent = Mathf.Lerp(percent, 0, lerpDelta);
                    // tutorialHandSplinePositioner.SetPercent(percent);
                    // tutorialHand.rotation = Quaternion.Euler(tutorialHandEulerAngles);
                }

                lerpProgress = Mathf.Lerp(lerpProgress, 1, lerpDelta);

                if (lerpProgress > 0.99f)
                {
                    phase++;
                    lerpProgress = 0;

                    if (phase == 1)
                    {
                        tutorialHandSplineFollower.follow = true;
                    }
                    // lerpDelta = 0.02f;
                }
            }
            // if (phase % 2 == 0)
            // {
            //     percent = Mathf.Lerp(percent, 1, lerpDelta);
            //     tutorialHandSplinePositioner.SetPercent(percent);
            //     tutorialHand.rotation = Quaternion.Euler(tutorialHandEulerAngles);
            // }
            // else
            // {
            //     percent = Mathf.Lerp(percent, 0, lerpDelta);
            //     tutorialHandSplinePositioner.SetPercent(percent);
            //     tutorialHand.rotation = Quaternion.Euler(tutorialHandEulerAngles);
            // }

            yield return waitForSeconds;
        }

        tutorialHandSplineFollower.follow = false;

        StartCoroutine(TutorialStepThree());
    }

    private IEnumerator TutorialStepThree()
    {
        float TIME_OUT = 5;

        WaitForSeconds waitForSeconds = new WaitForSeconds(Time.deltaTime);

        float timePassed = 0;

        // localizedTutorialText.TranslationName = translationNames[2];
        scaleTutorialText.gameObject.SetActive(false);
        pinchTutorialText.gameObject.SetActive(true);

        pinchHand.gameObject.SetActive(true);

        pinchHand.color = ColorUtil.WithAlpha(pinchHand.color, 0);

        Tween.Alpha(pinchHand, 1, duration: 0.2f);

        for (int i = 0; i < scaleIcons.Length; i++)
        {
            scaleIcons[i].gameObject.SetActive(true);
            scaleIcons[i].color = ColorUtil.WithAlpha(scaleIcons[i].color, 0);
        }

        Tween.Alpha(rotateIconWorld, 0, duration: 0.3f);
        Tween.Alpha(tutorialHandSprite, 0, duration: 0.3f);

        for (int i = 0; i < scaleIcons.Length; i++)
        {
            Tween.Alpha(scaleIcons[i], 1, duration: 0.3f);
        }

        Tween.Alpha(scaleIconHand, 1, duration: 0.3f)
        .OnComplete(() =>
        {
            enablePinchingEvent?.Invoke(true);
        });

        int phase = 0;

        RectTransform[] scaleIconRTs = new RectTransform[2];

        scaleIconRTs[0] = scaleIcons[0].GetComponent<RectTransform>();
        scaleIconRTs[1] = scaleIcons[1].GetComponent<RectTransform>();

        Vector3[] initialScaleIconPosition = new Vector3[2];

        initialScaleIconPosition[0] = scaleIconRTs[0].localPosition;
        initialScaleIconPosition[1] = scaleIconRTs[1].localPosition;

        float lerpProgress = 0;
        float lerpDelta = 0.12f;

        while (_step == 2 && timePassed < TIME_OUT)
        {
            if (phase % 2 == 0)
            {
                scaleIconRTs[0].localPosition = Vector3.Lerp(scaleIconRTs[0].localPosition, initialScaleIconPosition[0] + 60 * Vector3.one, lerpDelta);
                scaleIconRTs[1].localPosition = Vector3.Lerp(scaleIconRTs[1].localPosition, initialScaleIconPosition[1] - 60 * Vector3.one, lerpDelta);
            }
            else
            {
                scaleIconRTs[0].localPosition = Vector3.Lerp(scaleIconRTs[0].localPosition, initialScaleIconPosition[0] - 60 * Vector3.one, lerpDelta);
                scaleIconRTs[1].localPosition = Vector3.Lerp(scaleIconRTs[1].localPosition, initialScaleIconPosition[1] + 60 * Vector3.one, lerpDelta);
            }

            lerpProgress += lerpDelta;

            if (lerpProgress >= 1)
            {
                phase++;
                lerpProgress = 0;
            }

            yield return waitForSeconds;
        }

        EndTutorial();
    }

    private void EndTutorial()
    {
        Tween.Custom(1, 0, duration: 0.3f, onValueChange: newVal =>
        {
            canvasGroup.alpha = newVal;
            scaleIconHand.color = ColorUtil.WithAlpha(scaleIconHand.color, newVal);
        });
        Tween.Alpha(tutorialHandSprite, 0, duration: 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

        enableSwipingEvent?.Invoke(true);
    }

    #region CALLBACK
    private void OnScrewSelected()
    {
        if (_step == 0)
        {
            _step++;
        }
    }

    private void OnScreenSwiped(Vector2 swipeDirection)
    {
        if (_step == 1)
        {
            _step++;
        }
    }

    private void OnScreenZoomed(float orthographicSize)
    {
        if (_step == 2)
        {
            _step++;
        }
    }
    #endregion
}
