using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Saferio.Util
{
    public enum ScrollDirection
    {
        Up,
        Down
    }

    public class ObjectPoolingScrollView : MonoBehaviour
    {
        [Header("POOL PREFAB")]
        [SerializeField] private RectTransform prefab;

        #region ITEM
        private ISaferioScrollViewItem[] _scrollViewItems;
        #endregion

        [Header("UI")]
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform viewArea;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;

        private List<string> _cachedItemList;
        private List<RectTransform> _items;
        private List<TMP_Text> _texts;

        [Header("CUSTOMIZE")]
        [SerializeField] private int poolSize;

        #region PRIVATE FIELD
        private Vector2 _canvasSize;
        private Vector2 _viewAreaSize;
        private Vector2 _itemSize;
        private List<bool> _isItemFullyOutside;
        private float _maxScrollPositionY;
        private float _minScrollPositionY;
        private Vector2 _lastContentPosition;

        private int _topMostVisibleItemIndex;
        private int _bottomMostVisibleItemIndex;

        private float _currentHighestItemPositionY;
        private float _currentLowestItemPositionY;






        private ScrollDirection _lastScrollDirection;
        private int _distanceScrollFromDirectionChanged;
        #endregion

        #region PROPERTY
        public RectTransform ViewArea => viewArea;
        public List<RectTransform> Items
        {
            get => _items;
            set => _items = value;
        }
        #endregion




        private void Awake()
        {
            _cachedItemList = new List<string>();

            _canvasSize = canvas.sizeDelta;

            CreatePool();

            GenerateUI();

            StartCoroutine(ObjectPoolingScrolling());
        }

        private void CreatePool()
        {
            _items = new List<RectTransform>();
            _texts = new List<TMP_Text>();
            _isItemFullyOutside = new List<bool>();
            _scrollViewItems = new ISaferioScrollViewItem[poolSize];

            for (int i = 0; i < poolSize; i++)
            {
                _items.Add(Instantiate(prefab, content));
                _items[i].name = $"Item {i}";
                _texts.Add(_items[i].GetChild(0).GetComponent<TMP_Text>());

                _scrollViewItems[i] = _items[i].GetComponent<ISaferioScrollViewItem>();
            }
        }

        private void GenerateUI()
        {
            // UIUtil.SetSize(viewArea, 0.5f * _canvasSize.x, 0.7f * _canvasSize.y);

            ISaferioLayoutModifier layoutModifier = GetComponent<ISaferioLayoutModifier>();

            if (layoutModifier != null)
            {
                layoutModifier.Modify();
            }

            _viewAreaSize = viewArea.sizeDelta;

            _itemSize = new Vector2(0.9f * _viewAreaSize.x, 0.25f * _viewAreaSize.y);

            // UIUtil.SetSize(content, new Vector2(_canvasSize.x, 1.4f * (poolSize * _itemSize.y + (poolSize - 1) * 0.1f * _itemSize.y)));
            UIUtil.SetSize(content, _viewAreaSize);
            UIUtil.SetLocalPosition(content, Vector2.zero);

            for (int i = 0; i < _items.Count; i++)
            {
                UIUtil.SetSize(_items[i], _itemSize);



                int index = i;

                _items[i].GetComponent<ISaferioScrollViewItem>().Setup(index, content);

                if (i == 0)
                {
                    _itemSize = _items[0].sizeDelta;
                }



                UIUtil.SetLocalPositionY(_items[i], 0.5f * (_viewAreaSize.y - _items[i].sizeDelta.y) - 0.05f * _viewAreaSize.x - 1.1f * i * _items[i].sizeDelta.y);

                if
                (
                    _items[i].localPosition.y - 0.5f * _items[i].sizeDelta.y + content.localPosition.y > 0.5f * _viewAreaSize.y ||
                    _items[i].localPosition.y + 0.5f * _items[i].sizeDelta.y + content.localPosition.y < -0.5f * _viewAreaSize.y
                )
                {
                    _isItemFullyOutside.Add(true);
                }
                else
                {
                    _isItemFullyOutside.Add(false);
                }
            }

            _topMostVisibleItemIndex = 0;
            _bottomMostVisibleItemIndex = poolSize - 1;

            _maxScrollPositionY = (poolSize - _bottomMostVisibleItemIndex) * _itemSize.y;

            _maxScrollPositionY = (poolSize - 3) * _items[0].sizeDelta.y;
            _maxScrollPositionY = 0.5f * _viewAreaSize.y;
            _minScrollPositionY = 0;

            _currentHighestItemPositionY = _items.First().localPosition.y;
            _currentLowestItemPositionY = _items.Last().localPosition.y;
        }

        public void Refresh()
        {
            if (_scrollViewItems == null)
            {
                return;
            }

            for (int i = 0; i < poolSize; i++)
            {
                int index = i;

                if (_scrollViewItems[i] != null)
                {
                    _scrollViewItems[i].Refresh(index);
                }
            }
        }

        private IEnumerator ObjectPoolingScrolling()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

            yield return waitForSeconds;

            while (true)
            {

                ScrollBackToValidPosition(300);

                ScrollDirection scrollDirection = GetScrollDirection();

                for (int i = 0; i < poolSize; i++)
                {
                    bool isOutsideTop = false;
                    bool isOutsideBottom = false;

                    if (_items[i].localPosition.y - 0.5f * _items[i].sizeDelta.y + content.localPosition.y > 0.5f * _viewAreaSize.y)
                    {
                        int index = i;

                        if (IsHighestItem(index))
                        {
                            isOutsideTop = true;
                        }
                    }

                    if (_items[i].localPosition.y + 0.5f * _items[i].sizeDelta.y + content.localPosition.y < -0.5f * _viewAreaSize.y)
                    {
                        int index = i;

                        if (IsLowestItem(index))
                        {
                            isOutsideBottom = true;
                        }
                    }

                    if (isOutsideTop && scrollDirection == ScrollDirection.Up && _scrollViewItems[i].IsValidAtIndex(_bottomMostVisibleItemIndex + 1))
                    {
                        UIUtil.SetLocalPositionY(_items[i], _currentLowestItemPositionY - 1.1f * _items[i].sizeDelta.y);

                        _currentHighestItemPositionY -= 1.1f * _items[i].sizeDelta.y;
                        _currentLowestItemPositionY -= 1.1f * _items[i].sizeDelta.y;

                        _bottomMostVisibleItemIndex++;
                        _topMostVisibleItemIndex++;

                        _scrollViewItems[i].Refresh(_bottomMostVisibleItemIndex);

                        _maxScrollPositionY += 1.1f * _items[i].sizeDelta.y;

                        break;
                    }

                    if (isOutsideBottom && scrollDirection == ScrollDirection.Down && _scrollViewItems[i].IsValidAtIndex(_topMostVisibleItemIndex - 1))
                    {
                        UIUtil.SetLocalPositionY(_items[i], _currentHighestItemPositionY + 1.1f * _items[i].sizeDelta.y);

                        _currentHighestItemPositionY += 1.1f * _items[i].sizeDelta.y;
                        _currentLowestItemPositionY += 1.1f * _items[i].sizeDelta.y;

                        _bottomMostVisibleItemIndex--;
                        _topMostVisibleItemIndex--;

                        _scrollViewItems[i].Refresh(_bottomMostVisibleItemIndex);

                        _minScrollPositionY -= 1.1f * _items[i].sizeDelta.y;

                        break;
                    }

                    // if (isOutsideTop && scrollDirection == ScrollDirection.Up && IsValidScrollingNext(isScrollUp: true))
                    // {
                    //     UIUtil.SetLocalPositionY(_items[i], _currentLowestItemPositionY - 1.1f * _items[i].sizeDelta.y);

                    //     _currentHighestItemPositionY -= 1.1f * _items[i].sizeDelta.y;
                    //     _currentLowestItemPositionY -= 1.1f * _items[i].sizeDelta.y;

                    //     _bottomMostVisibleItemIndex++;
                    //     _topMostVisibleItemIndex++;

                    //     _scrollViewItems[i].Refresh(_bottomMostVisibleItemIndex);

                    //     _maxScrollPositionY += 1.1f * _items[i].sizeDelta.y;

                    //     break;
                    // }
                }



























                // #region SCROLL DIRECTION
                // bool isScrollUp = false;

                // if (content.localPosition.y > _lastContentPosition.y)
                // {
                //     isScrollUp = true;
                // }
                // else
                // {
                //     isScrollUp = false;
                // }

                // _lastContentPosition = content.localPosition;
                // #endregion

                // if (content.localPosition.y < _minScrollPositionY && scrollRect.vertical)
                // {
                //     scrollRect.vertical = false;

                //     Tween.LocalPositionY(content, _minScrollPositionY, duration: 0.3f).OnComplete(() =>
                //     {
                //         scrollRect.vertical = true;
                //     });

                //     yield return waitForSeconds;

                //     continue;
                // }

                // if (content.localPosition.y > _maxScrollPositionY && scrollRect.vertical)
                // {
                //     scrollRect.vertical = false;

                //     Tween.LocalPositionY(content, _maxScrollPositionY, duration: 0.3f).OnComplete(() =>
                //     {
                //         scrollRect.vertical = true;
                //     });

                //     yield return waitForSeconds;

                //     continue;
                // }

                // for (int i = 0; i < poolSize; i++)
                // {
                //     bool isItemFullyOutside = false;
                //     bool isOutsideTop = false;
                //     bool isOutsideBottom = false;

                //     if (_items[i].localPosition.y - 0.5f * _items[i].sizeDelta.y + content.localPosition.y > 0.5f * _viewAreaSize.y)
                //     {
                //         isItemFullyOutside = true;

                //         // if (_items[i].localPosition.y == _currentHighestItemPositionY)
                //         // {
                //         //     isOutsideTop = true;
                //         // }

                //         int index = i;

                //         if (IsHighestItem(index))
                //         {
                //             isOutsideTop = true;
                //         }
                //     }

                //     if (_items[i].localPosition.y + 0.5f * _items[i].sizeDelta.y + content.localPosition.y < -0.5f * _viewAreaSize.y)
                //     {
                //         isItemFullyOutside = true;

                //         // if (_items[i].localPosition.y == _currentLowestItemPositionY)
                //         // {
                //         //     isOutsideBottom = true;
                //         // }

                //         int index = i;

                //         if (IsLowestItem(index))
                //         {
                //             isOutsideBottom = true;
                //         }
                //     }

                //     // if (_isItemFullyOutside[i])
                //     // {
                //     //     _isItemFullyOutside[i] = isItemFullyOutside;

                //     //     continue;
                //     // }

                //     // Debug.Log("Item " + i);
                //     // Debug.Log(_items[i].localPosition.y + 0.5f * _items[i].sizeDelta.y + content.localPosition.y < -0.5f * _canvasSize.y);
                //     // Debug.Log(_items[i].localPosition.y == _currentLowestItemPositionY);
                //     // Debug.Log(isOutsideBottom + "/" + !isScrollUp + "/" + IsValidScrollingNext(isScrollUp: false));
                //     // Debug.Log("----------------");

                //     if (isOutsideTop && isScrollUp && IsValidScrollingNext(isScrollUp: true))
                //     {
                //         UIUtil.SetLocalPositionY(_items[i], _currentLowestItemPositionY - 1.1f * _items[i].sizeDelta.y);

                //         _currentHighestItemPositionY -= 1.1f * _items[i].sizeDelta.y;
                //         _currentLowestItemPositionY -= 1.1f * _items[i].sizeDelta.y;

                //         _bottomMostVisibleItemIndex++;
                //         _topMostVisibleItemIndex++;

                //         _scrollViewItems[i].Refresh(_bottomMostVisibleItemIndex);

                //         // _texts[i].text = _cachedItemList[_bottomMostVisibleItemIndex];
                //         // _texts[i].text = $"{areaDataContainer.AreaDatum[_bottomMostVisibleItemIndex].name}";

                //         break;
                //     }
                //     else if (isOutsideBottom && !isScrollUp && IsValidScrollingNext(isScrollUp: false))
                //     {
                //         UIUtil.SetLocalPositionY(_items[i], _currentHighestItemPositionY + 1.1f * _items[i].sizeDelta.y);

                //         _currentHighestItemPositionY += 1.1f * _items[i].sizeDelta.y;
                //         _currentLowestItemPositionY += 1.1f * _items[i].sizeDelta.y;

                //         _bottomMostVisibleItemIndex--;
                //         _topMostVisibleItemIndex--;

                //         _texts[i].text = _cachedItemList[_topMostVisibleItemIndex];
                //         // _texts[i].text = $"{areaDataContainer.AreaDatum[_topMostVisibleItemIndex].name}";

                //         break;
                //     }

                //     _isItemFullyOutside[i] = isItemFullyOutside;
                // }

                yield return waitForSeconds;
            }
        }

        #region UTIL
        private bool IsValidScrollingNext(bool isScrollUp)
        {
            if (isScrollUp)
            {
                return true;
                if (_bottomMostVisibleItemIndex + 1 < _cachedItemList.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_topMostVisibleItemIndex - 1 >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool IsHighestItem(int index)
        {
            int highestItemIndex = 0;
            float min = float.MaxValue;

            for (int i = 0; i < poolSize; i++)
            {
                float distance = Mathf.Abs(_items[i].localPosition.y - _currentHighestItemPositionY);
                if (distance < min)
                {
                    min = distance;

                    highestItemIndex = i;
                }
            }

            if (index == highestItemIndex)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsLowestItem(int index)
        {
            int lowestItemIndex = 0;
            float min = float.MaxValue;

            for (int i = 0; i < poolSize; i++)
            {
                float distance = Mathf.Abs(_items[i].localPosition.y - _currentLowestItemPositionY);
                if (distance < min)
                {
                    min = distance;

                    lowestItemIndex = i;
                }
            }

            if (index == lowestItemIndex)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion










        #region NEW
        private bool IsScrollable(float scrollOverLimitTolerance = 0)
        {
            bool isScrollable = true;

            if (content.localPosition.y < _minScrollPositionY - scrollOverLimitTolerance && scrollRect.vertical)
            {
                isScrollable = false;
            }

            if (content.localPosition.y > _maxScrollPositionY + scrollOverLimitTolerance && scrollRect.vertical)
            {
                isScrollable = false;
            }

            return isScrollable;
        }

        private void ScrollBackToValidPosition(float scrollOverLimitTolerance = 0)
        {
            if (content.localPosition.y < _minScrollPositionY - scrollOverLimitTolerance && scrollRect.vertical)
            {
                scrollRect.vertical = false;

                Tween.LocalPositionY(content, _minScrollPositionY, duration: 0.3f).OnComplete(() =>
                {
                    scrollRect.vertical = true;
                });
            }

            if (content.localPosition.y > _maxScrollPositionY + scrollOverLimitTolerance && scrollRect.vertical)
            {
                scrollRect.vertical = false;

                Tween.LocalPositionY(content, _maxScrollPositionY, duration: 0.3f).OnComplete(() =>
                {
                    scrollRect.vertical = true;
                });
            }
        }

        private ScrollDirection GetScrollDirection()
        {
            ScrollDirection scrollDirection = _lastScrollDirection;

            if (_lastScrollDirection == ScrollDirection.Up)
            {
                if (content.localPosition.y < _lastContentPosition.y)
                {
                    _distanceScrollFromDirectionChanged++;

                    if (_distanceScrollFromDirectionChanged > 5)
                    {
                        scrollDirection = ScrollDirection.Down;

                        _distanceScrollFromDirectionChanged = 0;
                    }
                }
            }
            else
            {
                if (content.localPosition.y > _lastContentPosition.y)
                {
                    _distanceScrollFromDirectionChanged++;

                    if (_distanceScrollFromDirectionChanged > 5)
                    {
                        scrollDirection = ScrollDirection.Up;

                        _distanceScrollFromDirectionChanged = 0;
                    }
                }
            }

            _lastContentPosition = content.localPosition;
            _lastScrollDirection = scrollDirection;

            return scrollDirection;
        }
        #endregion
    }
}
