using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace LFramework
{
    public class ViewContainer : MonoSingleton<ViewContainer>
    {
        private List<View> _views = new List<View>();

        private bool _isTransiting = false;

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += SceneManager_ActiveSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneManager_ActiveSceneChanged;
        }

        private void SceneManager_ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            // Clear all view when scene changed
            for (int i = _views.Count - 1; i >= 0; i--)
                _views[i].Close();
        }

        private View GetTopView()
        {
            return _views.Count <= 0 ? null : _views.Last();
        }

        private void PopTopView()
        {
            _views.Pop();
        }

        private void RevealTopView()
        {
            View topView = GetTopView();

            if (topView != null && topView.showOnReveal)
                topView.Show();
        }

        private void BlockTopView()
        {
            View topView = GetTopView();

            if (topView != null && topView.hideOnBlock)
                topView.Hide();
        }

        public async UniTask<View> PushAsync(AssetReference viewAsset)
        {
            // Can't push another view when it is transiting
            if (_isTransiting)
            {
                LDebug.Log<ViewContainer>($"Another View is transiting, can't push any new view {viewAsset}");
                return null;
            }

            // Get previous view (current top view)
            View previousView = GetTopView();

            // Disable interactable top view
            if (previousView != null)
                previousView.interactable = false;

            // Set transiting flag
            _isTransiting = true;

            // Wait new view to be loaded
            var handle = Addressables.InstantiateAsync(viewAsset, transformCached, false);
            await handle;
            View view = handle.Result.GetComponent<View>();

            BlockTopView();

            // Handle view callback
            view.onCloseStart.AddListener(PopTopView);
            view.onCloseEnd.AddListener(() =>
            {
                viewAsset.ReleaseInstance(view.gameObjectCached);
                RevealTopView();
            });

            // Open new view
            view.Open();

            // Push new view into stack
            _views.Add(view);

            // Unset transiting flag
            _isTransiting = false;

            return view;
        }
    }
}