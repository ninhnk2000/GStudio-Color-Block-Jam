using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameEnum;

public class BlockSelectionInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [Header("CUSTOMIZE")]
    [SerializeField] private LayerMask layerMaskCheck;
    [SerializeField] private float maxSwipeDistance;
    [SerializeField] private float timeToConfirmSelectObjectPart;

    #region PRIVATE FIELD
    [SerializeField] private InputMode _inputMode;
    private List<Vector3> twoTouchPoints;
    private Vector3 _prevTouchPosition;
    private BaseBlock _selectedBlock;
    #endregion

    #region EVENT
    public static event Action mouseUpEvent;
    public static event Action selectScrewEvent;
    public static event Action breakObjectEvent;
    #endregion

    void Awake()
    {
        GameStateMachine.enableInputEvent += EnableInput;

        twoTouchPoints = new List<Vector3>();
    }

    private void OnDrawGizmos()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Gizmos.DrawRay(ray);
    }

    void OnDestroy()
    {
        GameStateMachine.enableInputEvent -= EnableInput;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SelectBlock();

            MoveBlock();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _selectedBlock.Stop();

            _selectedBlock = null;
        }
    }

    private async void SelectBlock()
    {
        if (_selectedBlock != null)
        {
            return;
        }

        if (IsClickedOnUI())
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, 999);

        if (hit.collider != null)
        {
            BaseBlock block = hit.collider.GetComponent<BaseBlock>();

            if (block != null)
            {
                _selectedBlock = block;

                _prevTouchPosition = Input.mousePosition;
            }
        }
    }

    private void MoveBlock()
    {
        if (_selectedBlock != null)
        {
            Vector2 direction = Input.mousePosition - _prevTouchPosition;

            _selectedBlock.Move(direction.normalized);

            _prevTouchPosition = Input.mousePosition;
        }
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
