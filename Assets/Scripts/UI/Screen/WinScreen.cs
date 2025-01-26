using Saferio.Util.SaferioTween;
using UnityEngine;
using UnityEngine.UI;

namespace Saferio.Prototype.ColoringBook
{
    public class WinScreen : UIScreen
    {
        [SerializeField] private RectTransform continueButtonRT;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button replayButton;
        [SerializeField] private Button continueButton;

        [SerializeField] private Image fadeBackground;
        [SerializeField] private RectTransform winTextRT;

        protected override void RegisterMoreEvent()
        {

        }

        protected override void UnregisterMoreEvent()
        {

        }

        protected override void MoreActionInAwake()
        {
            gameObject.SetActive(false);
        }

        private void OnWin()
        {
            // gameObject.SetActive(true);

            // PlaySoundWin();

            // winTextRT.localPosition = new Vector2(0, canvasSize.Value.y);
            // continueButtonRT.localPosition = new Vector2(0, -canvasSize.Value.y);

            // winTextRT.gameObject.SetActive(true);
            // continueButtonRT.gameObject.SetActive(true);
            // fadeBackground.gameObject.SetActive(true);
            // fadeBackground.color = new Color(fadeBackground.color.r, fadeBackground.color.g, fadeBackground.color.b, 0);

            // SaferioTween.AlphaAsync(fadeBackground, 0.9f, duration: 0.5f);
            // SaferioTween.LocalPositionAsync(winTextRT, new Vector2(0, 0.2f * canvasSize.Value.y), duration: 0.5f);
            // SaferioTween.LocalPositionAsync(continueButtonRT, new Vector2(0, -0.2f * canvasSize.Value.y), duration: 0.5f);
        }

        private void PlaySoundWin()
        {
            AudioSource selectSound = ObjectPoolingEverything.GetFromPool<AudioSource>("WinSound");

            selectSound.gameObject.SetActive(true);

            selectSound.Play();
        }
    }
}

