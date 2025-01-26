using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CandyCoded.HapticFeedback;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static GameEnum;

public class ScrewBox : MonoBehaviour
{
    [SerializeField] private ScrewBoxSlot[] screwBoxSlots;
    [SerializeField] private Transform boxLid;
    [SerializeField] private SpriteRenderer screwBoxSpriteRenderer;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] private bool isLocked;
    [SerializeField] private float delayMoveInDuration;
    [SerializeField] private float closeBoxLidDuration;
    [SerializeField] private float scaleDownDuration;
    [SerializeField] private float delayMoveOutDuration;
    [SerializeField] private float moveOutDuration;

    #region EVENT
    public static event Action<ScrewBox> screwBoxCompletedEvent;
    public static event Action spawnNewScrewBoxEvent;
    public static event Action<ScrewBox> setFactionForScrewBoxEvent;
    public static event Action<ScrewBox> screwBoxUnlockedEvent;
    public static event Action shakeCameraEvent;
    #endregion

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Vector3 _initialScale;
    private bool _isInTransition;
    #endregion

    public GameFaction Faction
    {
        get => screwBoxServiceLocator.screwBoxFaction.Faction;
        set => screwBoxServiceLocator.screwBoxFaction.Faction = value;
    }

    public ScrewBoxSlot[] ScrewBoxSlots
    {
        get => screwBoxSlots;
    }

    public bool IsLocked
    {
        get => isLocked;
        set => isLocked = value;
    }

    public bool IsInTransition
    {
        get => _isInTransition;
        set => _isInTransition = value;
    }

    public ScrewBoxServiceLocator ScrewBoxServiceLocator
    {
        get => screwBoxServiceLocator;
    }

    private void Awake()
    {
        ScrewBoxCameraManager.setCameraEvent += OnScrewBoxCameraSet;
        ScrewBoxSlot.screwBoxCompleteEvent += OnScrewBoxCompleted;
        ScrewBoxUI.unlockScrewBox += Unlock;

        _tweens = new List<Tween>();

        boxLid.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ScrewBoxCameraManager.setCameraEvent -= OnScrewBoxCameraSet;
        ScrewBoxSlot.screwBoxCompleteEvent -= OnScrewBoxCompleted;
        ScrewBoxUI.unlockScrewBox -= Unlock;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void OnScrewBoxCameraSet(Camera screwBoxCamera)
    {
        Vector2 worldSize = CommonUtil.GetScreenSizeWorld(screwBoxCamera);

        float scaleWithWidth = (0.17f * worldSize.x) * transform.localScale.x / screwBoxSpriteRenderer.bounds.size.x;
        float scaleWithHeight = (0.113811f * worldSize.y) * transform.localScale.y / screwBoxSpriteRenderer.bounds.size.y;
        float finalScale = Mathf.Min(scaleWithWidth, scaleWithHeight);

        transform.localScale = finalScale * Vector3.one;

        _initialScale = transform.localScale;
    }

    private async void OnScrewBoxCompleted(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            await Task.Delay((int)(delayMoveInDuration * 1000));

            SoundManager.Instance.PlaySoundScrewBoxDone();

            float initialPositionY = transform.position.y;

            boxLid.gameObject.SetActive(true);

            boxLid.localPosition = boxLid.localPosition.ChangeY(9);

            Tween.LocalPositionY(boxLid, 0.4f, duration: closeBoxLidDuration)
            .OnComplete(() =>
            {
                shakeCameraEvent?.Invoke();
                // Tween.ShakeCamera(Camera.main, strengthFactor: 0.3f, duration: closeBoxLidDuration);

                HapticFeedback.HeavyFeedback();
            })
            .Chain(Tween.ScaleY(transform, 0.6f * _initialScale.y, duration: scaleDownDuration)
            .Chain(Tween.LocalPositionY(transform, transform.position.y + 8, startDelay: delayMoveOutDuration, duration: moveOutDuration).OnComplete(() =>
            {
                screwBoxCompletedEvent?.Invoke(this);
                spawnNewScrewBoxEvent?.Invoke();

                transform.position = transform.position.ChangeY(initialPositionY);
                transform.localScale = _initialScale;

                boxLid.gameObject.SetActive(false);

                ObjectPoolingEverything.ReturnToPool(GameConstants.SCREW_BOX, gameObject);
            })));
        }
    }

    public void Lock()
    {
        screwBoxServiceLocator.screwBoxUI.Lock();

        isLocked = true;
    }

    public void Unlock()
    {
        // CLEAR SCREWS
        for (int i = 0; i < screwBoxSlots.Length; i++)
        {
            if (screwBoxSlots[i].IsFilled)
            {
                screwBoxSlots[i].IsFilled = false;

                Destroy(screwBoxSlots[i].Screw.gameObject);

                screwBoxSlots[i].Screw = null;
            }
        }

        isLocked = false;

        setFactionForScrewBoxEvent?.Invoke(this);
        screwBoxUnlockedEvent?.Invoke(this);
    }

    private void Unlock(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Unlock();
        }
    }
}
