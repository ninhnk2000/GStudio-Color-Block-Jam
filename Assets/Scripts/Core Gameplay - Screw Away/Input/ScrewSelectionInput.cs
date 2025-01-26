using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameEnum;

public class ScrewSelectionInput : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private LayerMask layerMaskCheck;
    [SerializeField] private float maxSwipeDistance;
    [SerializeField] private float timeToConfirmSelectObjectPart;

    #region PRIVATE FIELD
    [SerializeField] private InputMode _inputMode;
    private List<Vector3> twoTouchPoints;
    private bool _isSelectObjectPart;
    private bool _isSelectingObjectPart;
    private float _timeHold;
    #endregion

    #region EVENT
    public static event Action mouseUpEvent;
    public static event Action selectScrewEvent;
    public static event Action breakObjectEvent;
    #endregion

    void Awake()
    {
        BoosterUI.enableBreakObjectModeEvent += EnableBreakObjectMode;
        BoosterUI.disableBreakObjectModeEvent += DisableBreakObjectMode;
        GameStateMachine.enableInputEvent += EnableInput;

        twoTouchPoints = new List<Vector3>();
    }

    void OnDestroy()
    {
        BoosterUI.enableBreakObjectModeEvent -= EnableBreakObjectMode;
        BoosterUI.disableBreakObjectModeEvent -= DisableBreakObjectMode;
        GameStateMachine.enableInputEvent -= EnableInput;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            twoTouchPoints.Add(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            _timeHold += Time.deltaTime;

            if (_inputMode == InputMode.Select)
            {
                if (_timeHold > timeToConfirmSelectObjectPart && !_isSelectingObjectPart)
                {
                    SelectObjectPart();

                    _isSelectingObjectPart = true;
                }
            }
            else if (_inputMode == InputMode.BreakObject)
            {
                SelectObjectPart();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            twoTouchPoints.Add(Input.mousePosition);

            if (twoTouchPoints.Count == 2)
            {
                if (Vector3.Distance(twoTouchPoints[0], twoTouchPoints[1]) < maxSwipeDistance)
                {
                    SelectScrew();
                }
            }

            mouseUpEvent?.Invoke();

            twoTouchPoints.Clear();

            _timeHold = 0;
            _isSelectObjectPart = false;
            _isSelectingObjectPart = false;
        }
    }

    private void SelectScrew()
    {
        if (_inputMode != InputMode.Select)
        {
            return;
        }

        if (_isSelectObjectPart)
        {
            return;
        }

        if (IsClickedOnUI())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, maxDistance: 999, layerMaskCheck);

        if (hit.collider != null)
        {
            IScrew screw = hit.collider.GetComponent<IScrew>();

            if (screw != null)
            {
                screw.Select();

                selectScrewEvent?.Invoke();

                return;
            }
        }
    }

    private async void SelectObjectPart()
    {
        _isSelectObjectPart = false;

        if (_inputMode == InputMode.Disabled)
        {
            return;
        }

        if (IsClickedOnUI())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, layerMaskCheck);

        if (hit.collider != null)
        {
            IObjectPart objectPart = hit.collider.GetComponent<IObjectPart>();

            if (objectPart != null)
            {
                _isSelectObjectPart = true;

                if (_inputMode == InputMode.BreakObject)
                {
                    objectPart.Break(hit.point);

                    breakObjectEvent?.Invoke();

                    _inputMode = InputMode.Disabled;

                    // avoid click screw immediately
                    await Task.Delay(500);

                    _inputMode = InputMode.Select;
                }
                else
                {
                    objectPart.Select();
                }

                return;
            }
        }
    }

    private void EnableBreakObjectMode()
    {
        _inputMode = InputMode.BreakObject;
    }

    private void DisableBreakObjectMode()
    {
        _inputMode = InputMode.Select;
    }

    private void EnableInput(bool isEnable)
    {
        if (isEnable)
        {
            _inputMode = InputMode.Select;
        }
        else
        {
            _inputMode = InputMode.Disabled;
        }
    }

    private bool IsClickedOnUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
