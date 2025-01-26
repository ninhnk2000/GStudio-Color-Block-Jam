using System.Collections;
using Google.Play.Review;
using Unity.VisualScripting;
using UnityEngine;

public class InAppReviewManager : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;

    private void Awake()
    {
        RatePopup.launchInAppReviewEvent += LaunchInAppReview;

        _reviewManager = new ReviewManager();
    }

    private void OnDestroy()
    {
        RatePopup.launchInAppReviewEvent += LaunchInAppReview;
    }

    private void LaunchInAppReview()
    {
        StartCoroutine(LaunchingInAppReview());
    }

    private IEnumerator LaunchingInAppReview()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();

        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();



        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);

        yield return launchFlowOperation;

        _playReviewInfo = null;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            yield break;
        }
    }
}
