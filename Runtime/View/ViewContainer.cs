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
            {
                _views[i].Close();
                _views.RemoveAt(i);
            }
        }

        private View GetTopView()
        {
            return _views.Count <= 0 ? null : _views.Last();
        }

        private void Pop()
        {
            _views.Pop();
        }

        private void HandleTopView()
        {
            View topView = GetTopView();

            if (topView != null)
                topView.Show();
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

            // Handle view callback
            view.onCloseStart.AddListener(Pop);
            view.onCloseEnd.AddListener(() =>
            {
                viewAsset.ReleaseInstance(view.gameObjectCached);
                HandleTopView();
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