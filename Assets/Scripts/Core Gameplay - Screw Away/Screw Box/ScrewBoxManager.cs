using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static GameEnum;

public class ScrewBoxManager : MonoBehaviour
{
    [SerializeField] private ScrewBox[] screwBoxs;
    [SerializeField] private List<ScrewBoxSlot> screwPorts;
    [SerializeField] private Transform screwPortsContainer;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private ScrewBoxesObserver screwBoxesObserver;

    [Header("CUSTOMIZE")]
    [SerializeField] private int maxScrewBox;

    public ScrewBox[] ScrewBoxs
    {
        get => screwBoxs;
    }

    public List<ScrewBoxSlot> ScrewPorts
    {
        get => screwPorts;
    }

    #region PRIVATE FIELD
    private float _screwBoxPositionY = 10;
    private bool _isSpawnScrewBoxFirstTimeInLevel;
    private float _screwBoxDistance;
    private bool _isScrewPortsShaking;
    #endregion

    #region EVENT
    public static event Action<int, GameFaction, ScrewBoxSlot> looseScrewEvent;
    public static event Action<int, GameFaction, ScrewBoxSlot> looseScrewWithoutCheckingObstaclesEvent;
    public static event Action<ScrewBox> screwBoxSpawnedEvent;
    public static event Action loseLevelEvent;
    public static event Action<int> screwPortsClearedEvent;
    public static event Action<bool> enableBoosterEvent;
    #endregion

    #region LIFE CYCLE
    void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStart;
        BaseScrew.selectScrewEvent += OnScrewSelected;
        BaseScrew.screwMovedCompletedEvent += MoveSingleScrewFromHoleToScrewBox;
        ScrewManager.spawnScrewBoxEvent += SpawnScrewBox;
        ScrewBoxCameraManager.setCameraEvent += OnScrewBoxCameraSet;
        ScrewManager.spawnAdsScrewBoxesEvent += SpawnAdsScrewBoxes;
        ScrewBox.screwBoxCompletedEvent += OnScrewBoxCompleted;
        ScrewBox.screwBoxUnlockedEvent += OnScrewBoxUnlocked;
        BoosterUI.addMoreScrewPortEvent += AddMoreScrewPort;
        BoosterUI.clearAllScrewPortsEvent += ClearAllScrewPorts;
        BoosterUI.shakeScrewPortsEvent += ShakeAllScrewPorts;
        BasicObjectPart.loosenScrewOnObjectBrokenEvent += LoosenScrewOnObjectBroken;
        RevivePopup.reviveEvent += OnRevived;
        BuyBoosterPopup.unlockScrewBoxEvent += OnRewardedAdUnlockScrewBoxCompleted;
    }

    void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStart;
        BaseScrew.selectScrewEvent -= OnScrewSelected;
        BaseScrew.screwMovedCompletedEvent -= MoveSingleScrewFromHoleToScrewBox;
        ScrewBoxCameraManager.setCameraEvent -= OnScrewBoxCameraSet;
        ScrewManager.spawnScrewBoxEvent -= SpawnScrewBox;
        ScrewManager.spawnAdsScrewBoxesEvent -= SpawnAdsScrewBoxes;
        ScrewBox.screwBoxCompletedEvent -= OnScrewBoxCompleted;
        ScrewBox.screwBoxUnlockedEvent -= OnScrewBoxUnlocked;
        BoosterUI.addMoreScrewPortEvent -= AddMoreScrewPort;
        BoosterUI.clearAllScrewPortsEvent -= ClearAllScrewPorts;
        BoosterUI.shakeScrewPortsEvent -= ShakeAllScrewPorts;
        BasicObjectPart.loosenScrewOnObjectBrokenEvent -= LoosenScrewOnObjectBroken;
        RevivePopup.reviveEvent -= OnRevived;
        BuyBoosterPopup.unlockScrewBoxEvent -= OnRewardedAdUnlockScrewBoxCompleted;

        ReturnScrewBoxesToPool();
    }
    #endregion

    private void ReturnScrewBoxesToPool()
    {
        if (screwBoxs != null)
        {
            for (int i = 0; i < screwBoxs.Length; i++)
            {
                if (screwBoxs[i] != null)
                {
                    for (int j = 0; j < screwBoxs[i].ScrewBoxSlots.Length; j++)
                    {
                        if (screwBoxs[i].ScrewBoxSlots[j].Screw != null)
                        {
                            screwBoxs[i].ScrewBoxSlots[j].IsFilled = false;

                            Destroy(screwBoxs[i].ScrewBoxSlots[j].Screw.gameObject);
                        }
                    }

                    ObjectPoolingEverything.ReturnToPool(GameConstants.SCREW_BOX, screwBoxs[i].gameObject);
                }
            }
        }
    }

    private void ResetScrewBoxes()
    {
        ReturnScrewBoxesToPool();

        screwBoxs = new ScrewBox[maxScrewBox];

        // RESET SCREW PORTS
        for (int i = 0; i < screwPorts.Count; i++)
        {
            if (screwPorts[i].IsFilled)
            {
                screwPorts[i].IsFilled = false;

                Destroy(screwPorts[i].Screw.gameObject);

                screwPorts[i].Screw = null;
            }
        }

        int toRemoveStartIndex = 0;
        int toRemoveCount = 0;

        for (int i = 0; i < screwPorts.Count; i++)
        {
            if (i >= GameConstants.DEFAULT_NUMBER_SCREW_PORT)
            {
                ObjectPoolingEverything.ReturnToPool(GameConstants.SCREW_PORT_SLOT, screwPorts[i].gameObject);

                if (toRemoveStartIndex == 0)
                {
                    toRemoveStartIndex = i;
                }

                toRemoveCount++;
            }
        }

        screwPorts.RemoveRange(toRemoveStartIndex, toRemoveCount);

        Vector3 screwPortPosition = screwPorts[0].transform.position;

        for (int i = 0; i < screwPorts.Count; i++)
        {
            screwPortPosition.x = (-(screwPorts.Count - 1) / 2f + i) * 1.08f;

            screwPorts[i].transform.position = screwPortPosition;
        }

        screwBoxesObserver.NumLockedScrewBoxes = GameConstants.DEFAULT_INITIAL_NUMBER_SCREW_BOXES;
        screwBoxesObserver.NumScrewInScrewPorts = 0;
    }

    #region CALLBACK
    private void OnLevelStart()
    {
        ResetScrewBoxes();

        _isSpawnScrewBoxFirstTimeInLevel = true;
    }

    private void OnScrewBoxCameraSet(Camera screwBoxCamera)
    {
        Vector2 worldSize = CommonUtil.GetScreenSizeWorld(screwBoxCamera);

        _screwBoxDistance = 0.213f * worldSize.x;

        _screwBoxPositionY = 0.42f * worldSize.y;

        screwPortsContainer.localPosition = screwPortsContainer.localPosition.ChangeY(0.3042f * worldSize.y);
    }

    private void OnRevived(BoosterType boosterType)
    {
        // if (boosterType == BoosterType.UnlockScrewBox)
        // {
        //     for (int i = 0; i < screwBoxs.Length; i++)
        //     {
        //         if (screwBoxs[i].IsLocked)
        //         {
        //             screwBoxs[i].ScrewBoxServiceLocator.screwBoxUI.Unlock();

        //             break;
        //         }
        //     }
        // }
        // else if (boosterType == BoosterType.ClearScrewPorts)
        // {
        //     ClearAllScrewPorts();
        // }
    }

    private void OnScrewSelected(int screwId, GameFaction selectedFaction)
    {
        ScrewBoxSlot screwBoxSlot = CheckAvailableScrewBoxes(selectedFaction);

        if (screwBoxSlot != null)
        {
            looseScrewEvent?.Invoke(screwId, selectedFaction, screwBoxSlot);
        }
    }

    private void OnScrewBoxCompleted(ScrewBox screwBox)
    {
        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == screwBox)
            {
                screwBoxs[i] = null;

                break;
            }
        }
    }

    private void OnScrewBoxUnlocked(ScrewBox screwBox)
    {
        MoveFromScrewPortToScrewBox(screwBox);

        screwBoxSpawnedEvent?.Invoke(screwBox);

        int numLockedScrewBoxes = 0;

        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i].IsLocked)
            {
                numLockedScrewBoxes++;
            }
        }

        screwBoxesObserver.NumLockedScrewBoxes = numLockedScrewBoxes;
    }

    private void OnRewardedAdUnlockScrewBoxCompleted()
    {
        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                continue;
            }

            if (screwBoxs[i].IsLocked)
            {
                screwBoxs[i].ScrewBoxServiceLocator.screwBoxUI.Unlock();

                break;
            }
        }
    }
    #endregion

    public ScrewBoxSlot CheckAvailableScrewBoxes(GameFaction selectedFaction)
    {
        return CheckAvailableScrewBoxes(selectedFaction, isIncludeScrewPorts: true);
    }

    public ScrewBoxSlot CheckAvailableScrewBoxes(GameFaction selectedFaction, bool isIncludeScrewPorts)
    {
        foreach (var screwBox in screwBoxs)
        {
            if (screwBox == null)
            {
                continue;
            }

            if (screwBox.IsLocked || screwBox.IsInTransition)
            {
                continue;
            }

            if (selectedFaction == screwBox.Faction)
            {
                foreach (var screwBoxSlot in screwBox.ScrewBoxSlots)
                {
                    if (!screwBoxSlot.IsFilled)
                    {
                        return screwBoxSlot;
                    }
                }
            }
        }

        ScrewBoxSlot emptyScrewPort = null;
        int numScrewPortsEmpty = 0;

        if (isIncludeScrewPorts)
        {
            foreach (var screwPort in screwPorts)
            {
                if (!screwPort.IsFilled)
                {
                    if (numScrewPortsEmpty == 0)
                    {
                        emptyScrewPort = screwPort;
                    }

                    numScrewPortsEmpty++;
                }
            }

            if (numScrewPortsEmpty == 1)
            {
                loseLevelEvent?.Invoke();
            }
            else
            {
                screwBoxesObserver.NumScrewInScrewPorts++;

                return emptyScrewPort;
            }
        }

        return null;
    }

    #region SPAWN SCREW BOX
    public void SpawnScrewBox(GameFaction faction)
    {
        SpawnScrewBox(faction, false);
    }

    public void SpawnScrewBox(GameFaction faction, bool isLocked = false)
    {
        if (isLocked)
        {
            SpawnAdsScrewBox();

            return;
        }

        ScrewBox screwBox = ObjectPoolingEverything.GetFromPool<ScrewBox>(GameConstants.SCREW_BOX);

        screwBox.Faction = faction;



        int index = 0;

        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                screwBoxs[i] = screwBox;

                index = i;

                break;
            }
        }

        Direction getInDirection = index > 1 ? Direction.Left : Direction.Right;

        screwBox.transform.position = new Vector3(getInDirection == Direction.Right ? -10 : 10, _screwBoxPositionY, screwBox.transform.position.z);

        ResetScrewBoxSlot();

        Vector3 destination = screwBox.transform.position;

        destination.x = (-(maxScrewBox - 1) / 2f + index) * _screwBoxDistance;
        destination.z = -0.001f * index;

        if (_isSpawnScrewBoxFirstTimeInLevel)
        {
            screwBox.transform.position = destination;

            MoveFromScrewPortToScrewBox(screwBox);
        }
        else
        {
            screwBox.IsInTransition = true;

            Tween.LocalPosition(screwBox.transform, destination, duration: 0.5f).OnComplete(() =>
            {
                screwBox.IsInTransition = false;

                MoveFromScrewPortToScrewBox(screwBox);

                screwBoxSpawnedEvent?.Invoke(screwBox);
            });
        }

        void ResetScrewBoxSlot()
        {
            for (int i = 0; i < screwBox.ScrewBoxSlots.Length; i++)
            {
                ScrewBoxSlot screwBoxSlot = screwBox.ScrewBoxSlots[i];

                if (screwBoxSlot.IsFilled)
                {
                    screwBoxSlot.Screw.gameObject.SetActive(false);

                    screwBoxSlot.Screw = null;

                    screwBoxSlot.IsFilled = false;
                }
            }
        }
    }

    public void SpawnScrewBox(ScrewBoxData screwBoxData)
    {

    }

    private void SpawnAdsScrewBox()
    {
        Vector3 destination;

        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                ScrewBox screwBox = ObjectPoolingEverything.GetFromPool<ScrewBox>(GameConstants.SCREW_BOX);

                screwBox.transform.position = new Vector3(10, _screwBoxPositionY, screwBox.transform.position.z);

                screwBox.Lock();

                int index = i;

                destination = screwBox.transform.position;
                destination.x = (-(maxScrewBox - 1) / 2f + index) * _screwBoxDistance;
                destination.z = -0.001f * index;

                Tween.LocalPosition(screwBox.transform, destination, duration: 0.5f)
                .OnComplete(() =>
                {
                    screwBox.ScrewBoxServiceLocator.screwBoxUI.SetUnlockByAdsButtonPosition();
                });

                screwBoxs[i] = screwBox;

                break;
            }
        }
    }

    private void SpawnAdsScrewBoxes()
    {
        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                int index = i;

                ScrewBox screwBox = ObjectPoolingEverything.GetFromPool<ScrewBox>(GameConstants.SCREW_BOX);

                screwBox.transform.position = new Vector3(10, _screwBoxPositionY, screwBox.transform.position.z);

                screwBox.Lock();

                Vector3 destination = screwBox.transform.position;

                destination.x = (-(maxScrewBox - 1) / 2f + index) * _screwBoxDistance;
                destination.z = -0.001f * index;

                if (_isSpawnScrewBoxFirstTimeInLevel)
                {
                    screwBox.transform.position = destination;

                    screwBox.ScrewBoxServiceLocator.screwBoxUI.SetUnlockByAdsButtonPosition();
                }
                else
                {
                    Tween.LocalPosition(screwBox.transform, destination, duration: 0.5f)
                    .OnComplete(() =>
                    {
                        screwBox.ScrewBoxServiceLocator.screwBoxUI.SetUnlockByAdsButtonPosition();
                    });
                }

                screwBoxs[i] = screwBox;
            }
        }

        _isSpawnScrewBoxFirstTimeInLevel = false;
    }
    #endregion

    private void MoveFromScrewPortToScrewBox(ScrewBox screwBox)
    {
        // avoid too fast action
        enableBoosterEvent?.Invoke(false);

        bool isFound = false;

        for (int i = 0; i < screwPorts.Count; i++)
        {
            BaseScrew screw = screwPorts[i].Screw;

            if (screwPorts[i].IsFilled && screw.Faction == screwBox.Faction && !screw.IsMoving)
            {
                for (int k = 0; k < screwBox.ScrewBoxSlots.Length; k++)
                {
                    if (!screwBox.ScrewBoxSlots[k].IsFilled)
                    {
                        int screwPortIndex = i;
                        int screwBoxSlotIndex = k;

                        screwBox.ScrewBoxSlots[screwBoxSlotIndex].Fill(screwPorts[screwPortIndex].Screw);

                        screwPorts[screwPortIndex].IsFilled = false;

                        screwPorts[screwPortIndex].Screw.UnscrewAnimationAsync
                        (
                            screwBoxSlot: screwBox.ScrewBoxSlots[screwBoxSlotIndex],
                            onCompleteAction: () =>
                            {
                                screwBox.ScrewBoxSlots[screwBoxSlotIndex].CompleteFill();

                                enableBoosterEvent?.Invoke(true);
                            },
                            isSwitchCamera: false
                        );

                        if (!isFound)
                        {
                            isFound = true;
                        }

                        break;
                    }
                }
            }
        }

        screwBoxesObserver.NumScrewInScrewPorts = GetNumScrewsInScrewPorts();

        if (!isFound)
        {
            enableBoosterEvent?.Invoke(true);
        }
    }

    private void MoveSingleScrewFromHoleToScrewBox(BaseScrew screw)
    {
        if (!screw.ScrewBoxSlot.IsScrewPort)
        {
            return;
        }

        bool isFound = false;

        for (int i = 0; i < screwBoxs.Length; i++)
        {
            ScrewBox screwBox = screwBoxs[i];

            if (screwBox == null)
            {
                continue;
            }

            if (screwBox.Faction != screw.Faction || screwBox.IsLocked || screwBox.IsInTransition)
            {
                continue;
            }

            for (int k = 0; k < screwBox.ScrewBoxSlots.Length; k++)
            {
                if (!screwBox.ScrewBoxSlots[k].IsFilled)
                {
                    int screwBoxSlotIndex = k;

                    screwBox.ScrewBoxSlots[screwBoxSlotIndex].Fill(screw);

                    screw.ScrewBoxSlot.IsFilled = false;

                    screw.UnscrewAnimationAsync
                    (
                        screwBoxSlot: screwBox.ScrewBoxSlots[screwBoxSlotIndex],
                        onCompleteAction: () =>
                        {
                            screwBox.ScrewBoxSlots[screwBoxSlotIndex].CompleteFill();

                            enableBoosterEvent?.Invoke(true);
                        },
                        isSwitchCamera: false
                    );

                    isFound = true;

                    break;
                }
            }

            if (isFound)
            {
                break;
            }
        }
    }

    public Dictionary<GameFaction, int> GetScrewPortAvailableByFaction()
    {
        Dictionary<GameFaction, int> screwPortAvailableByFaction = new Dictionary<GameFaction, int>();

        for (int i = 0; i < GameConstants.SCREW_FACTION.Length; i++)
        {
            screwPortAvailableByFaction.Add(GameConstants.SCREW_FACTION[i], 0);
        }

        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null || screwBoxs[i].IsLocked)
            {
                continue;
            }

            GameFaction faction = screwBoxs[i].Faction;

            for (int j = 0; j < screwBoxs[i].ScrewBoxSlots.Length; j++)
            {
                ScrewBoxSlot screwBoxSlot = screwBoxs[i].ScrewBoxSlots[j];

                if (!screwBoxSlot.IsFilled || screwBoxs[i].IsInTransition)
                {
                    screwPortAvailableByFaction[faction]++;
                }
            }
        }

        return screwPortAvailableByFaction;
    }

    #region BOOSTER
    private void LoosenScrewOnObjectBroken(BaseScrew screw)
    {
        ScrewBoxSlot screwBoxSlot = CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: false);

        if (screwBoxSlot != null)
        {
            looseScrewWithoutCheckingObstaclesEvent?.Invoke(screw.ScrewId, screw.Faction, screwBoxSlot);
        }
        else
        {
            screw.ForceUnscrew();
        }
    }

    public void AddMoreScrewPort()
    {
        ScrewBoxSlot addedScrewBoxSlot = ObjectPoolingEverything.GetFromPool<ScrewBoxSlot>(GameConstants.SCREW_PORT_SLOT);

        screwPorts.Add(addedScrewBoxSlot);

        addedScrewBoxSlot.transform.SetParent(screwPorts[0].transform.parent);

        addedScrewBoxSlot.transform.localRotation = Quaternion.Euler(Vector3.zero);
        addedScrewBoxSlot.transform.localScale = screwPorts[0].transform.localScale;

        Vector3 position = screwPorts[0].transform.position;

        // addedScrewBoxSlot.transform.position = position + new Vector3(10, 0, 0);
        addedScrewBoxSlot.transform.localScale = Vector3.zero;

        for (int i = 0; i < screwPorts.Count; i++)
        {
            int index = i;

            position.x = (-(screwPorts.Count - 1) / 2f + i) * 1.08f;

            Tween.Position(screwPorts[i].transform, position, duration: 0.3f)
            .OnComplete(() =>
            {
                if (index == screwPorts.Count - 1)
                {
                    Tween.Scale(addedScrewBoxSlot.transform, 1.15f * Vector3.one, duration: 0.3f);
                }
            });

            // screwPorts[i].transform.position = position;
        }
    }

    private void ClearAllScrewPorts()
    {
        int numScrewCleared = 0;

        for (int i = 0; i < screwPorts.Count; i++)
        {
            if (screwPorts[i].IsFilled)
            {
                if (screwPorts[i].Screw.IsMoving)
                {
                    continue;
                }

                screwPorts[i].IsFilled = false;

                screwPorts[i].Screw.ForceUnscrew();

                screwPorts[i].Screw = null;

                numScrewCleared++;
            }
        }

        screwBoxesObserver.NumScrewInScrewPorts = GetNumScrewsInScrewPorts();

        screwPortsClearedEvent?.Invoke(numScrewCleared);
    }

    private int GetNumScrewsInScrewPorts()
    {
        int numScrew = 0;

        for (int i = 0; i < screwPorts.Count; i++)
        {
            if (screwPorts[i].IsFilled)
            {
                numScrew++;
            }
        }

        return numScrew;
    }

    private void ShakeAllScrewPorts()
    {
        if (_isScrewPortsShaking)
        {
            return;
        }

        for (int i = 0; i < screwPorts.Count; i++)
        {
            bool isLast = i == screwPorts.Count - 1;

            Tween.PunchScale(screwPorts[i].transform, strength: 0.3f * Vector3.one, duration: 0.4f)
            .OnComplete(() =>
            {
                if (isLast)
                {
                    _isScrewPortsShaking = false;
                }
            });
        }

        _isScrewPortsShaking = true;
    }
    #endregion
}
