using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Saferio.Util.SaferioTween
{
    public class SaferioTweenManager : MonoBehaviour
    {
        private static SaferioTweenManager instance;

        [SerializeField] private List<string> lists;
        private Dictionary<int, Coroutine> _listCoroutineWithId;
        private int _currentId;

        public List<string> ListTween => lists;

        public static CancellationTokenSource CancellationTokenSourceOnDestroyed = new CancellationTokenSource();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            lists = new List<string>();
            _listCoroutineWithId = new Dictionary<int, Coroutine>();

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (!CancellationTokenSourceOnDestroyed.IsCancellationRequested)
            {
                CancellationTokenSourceOnDestroyed.Cancel();
                CancellationTokenSourceOnDestroyed.Dispose();
            }
        }

        public static int RunCoroutine(IEnumerator tween, object target)
        {
            return RunCoroutine(tween);
        }

        public static int RunCoroutine(IEnumerator tween)
        {
            Coroutine coroutine = instance.StartCoroutine(tween);

            instance._listCoroutineWithId.Add(instance._currentId, coroutine);

            return instance.GenerateID();
        }

        public IEnumerator TryingRunCoroutine(IEnumerator tween)
        {
            yield return new WaitUntil(() => instance != null);

            instance.StartCoroutine(tween);
        }

        public static void StopCoroutine(int id)
        {
            instance.StopCoroutine(instance._listCoroutineWithId[id]);
        }

        #region ASYNC

        #endregion

        #region UTIL
        private int GenerateID()
        {
            int cachedId = _currentId;

            _currentId++;

            return cachedId;
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SaferioTweenManager))]
    public class SaferioTweenManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaferioTweenManager saferioTweenManager = (SaferioTweenManager)target;

            EditorGUI.BeginDisabledGroup(true);

            if (saferioTweenManager.ListTween != null)
            {
                foreach (var item in saferioTweenManager.ListTween)
                {
                    EditorGUILayout.LabelField(item.ToString());
                }
            }

            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}