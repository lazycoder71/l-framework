
using UnityEngine;

#if UNITY_ANDROID && LFRAMEWORK_APPSTORE
using Google.Play.Review;
using Cysharp.Threading.Tasks;
using System.Threading;
#endif

namespace LFramework.AppStore
{
    public static class AppStore
    {
#if UNITY_ANDROID && LFRAMEWORK_APPSTORE
        private static ReviewManager s_reviewManager;
        private static PlayReviewInfo s_playReviewInfo;
        private static CancellationTokenSource s_cts;

        private static async UniTask InitAndroidReview()
        {
            if (s_reviewManager == null)
                s_reviewManager = new ReviewManager();

            var requestFlowOperation = s_reviewManager.RequestReviewFlow();

            await requestFlowOperation.ToUniTask(cancellationToken: s_cts.Token);

            if (requestFlowOperation.Error == ReviewErrorCode.NoError)
                s_playReviewInfo = requestFlowOperation.GetResult();
            else
                s_playReviewInfo = null;
        }

        public static async UniTask LaunchAndroidReview()
        {
            if (s_playReviewInfo == null)
                await InitAndroidReview();

            if (s_playReviewInfo == null)
            {
                OpenStore();
                return;
            }

            var launchFlowOperation = s_reviewManager.LaunchReviewFlow(s_playReviewInfo);

            await launchFlowOperation.ToUniTask(cancellationToken: s_cts.Token);

            s_playReviewInfo = null;

            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
                OpenStore();
        }
#endif

        public static void Init()
        {
#if UNITY_ANDROID && LFRAMEWORK_APPSTORE
            s_cts?.Cancel();
            s_cts = new CancellationTokenSource();

            InitAndroidReview().Forget();
#endif
        }

#if UNITY_IOS || UNITY_IPHONE
        public static void OpenStore(string appStoreID)
        {
            Application.OpenURL($"https://apps.apple.com/us/app/id{appStoreID}");
        }

        public static void Review(string appStoreID)
        {
            if (!UnityEngine.iOS.Device.RequestStoreReview())
                OpenStore(appStoreID);
        }
#else
        public static void OpenStore()
        {
            Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
        }

        public static void Review()
        {
#if UNITY_ANDROID && LFRAMEWORK_APPSTORE
            s_cts?.Cancel();
            s_cts = new CancellationTokenSource();

            LaunchAndroidReview().Forget();
#else
            OpenStore();
#endif
        }
#endif
    }
}