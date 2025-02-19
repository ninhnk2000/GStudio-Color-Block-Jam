using System;
using System.Collections;
using UnityEngine;
using static GameEnum;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform content;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] protected Vector2Variable canvasSizeOfReferenceDevice;

    [Header("CUSTOMIZE")]
    [SerializeField] private ScreenRoute route;
    [SerializeField] private bool isRouteActiveFromStart;

    #region PRIVATE FIELD
    private Vector3 _initialContainerPosititon;
    protected ISaferioUIAnimation _transitionAnimation;
    private bool _isShown;
    #endregion

    public RectTransform Container
    {
        get => container;
    }

    #region ACTION
    public static event Action<float> moveSwipingScreenEvent;
    #endregion  

    private void Awake()
    {
        SwitchRouteButton.switchRouteEvent += OnRouteSwitched;
        BottomBarItem.switchRouteEvent += OnRouteSwitched;
        SwipingScreen.hideOutsideScreenEvent += HideOutsideScreen;
        MenuScreen.switchRouteEvent += OnRouteSwitched;

        RegisterMoreEvent();

        _initialContainerPosititon = container.localPosition;
        _transitionAnimation = GetComponent<ISaferioUIAnimation>();

        if (isRouteActiveFromStart)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        MoreActionInAwake();
    }

    private void OnDestroy()
    {
        SwitchRouteButton.switchRouteEvent -= OnRouteSwitched;
        BottomBarItem.switchRouteEvent -= OnRouteSwitched;
        SwipingScreen.hideOutsideScreenEvent -= HideOutsideScreen;
        MenuScreen.switchRouteEvent -= OnRouteSwitched;

        UnregisterMoreEvent();
    }

    protected virtual void RegisterMoreEvent()
    {

    }

    protected virtual void UnregisterMoreEvent()
    {

    }

    protected virtual void MoreActionInAwake()
    {

    }

    public void SetInitialPosition(Vector3 position)
    {
        container.localPosition = position;

        _initialContainerPosititon = position;
    }

    private void OnRouteSwitched(ScreenRoute route)
    {
        if (route == this.route)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    protected virtual void Show()
    {
        content.gameObject.SetActive(true);
        
        moveSwipingScreenEvent?.Invoke(_initialContainerPosititon.x);

        _isShown = true;

        // _transitionAnimation.Show();
    }

    protected virtual void Hide()
    {
        if (_isShown)
        {
            // _transitionAnimation.Hide();

            _isShown = false;
        }
    }

    private void HideOutsideScreen(float swipingScreenPositionX)
    {
        if (Mathf.Abs(swipingScreenPositionX + container.localPosition.x) > canvasSizeOfReferenceDevice.Value.x)
        {
            content.gameObject.SetActive(false);
        }
        else
        {
            content.gameObject.SetActive(true);
        }
    }
}
