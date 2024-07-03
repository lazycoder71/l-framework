using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace LFramework.View
{
    public class ViewContainer : MonoSingleton<ViewContainer>
    {
        private Stack<View> _views = new Stack<View>();

        private bool _isTransiting = false;

        public async UniTask<View> PushAsync(AssetReference viewAsset)
        {
            // Can't push another view when it is transiting
            if (_isTransiting)
            {
                LDebug.Log<ViewContainer>($"Views are transiting, can't push new view {viewAsset}");
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
            View view = (await viewAsset.InstantiateAsync(transformCached, false).Task.AsUniTask()).GetComponent<View>();

            view.onCloseStart.AddListener(() => { PopAsync().Forget(); });
            view.onCloseEnd.AddListener(() => { viewAsset.ReleaseInstance(view.gameObjectCached); });

            // If new view created is page, hide previous view
            if (view.type == ViewType.Page && previousView != null)
                previousView.Hide();

            // Open new view
            view.Open();

            // Push new view into stack
            _views.Push(view);

            // Unset transiting flag
            _isTransiting = false;

            return view;
        }

        public async UniTask PopAsync()
        {
            View popedView = _views.Pop();

            View topView = GetTopView();

            if (popedView.type == ViewType.Page && topView != null)
            {
                await UniTask.WaitUntil(() => topView.isTransiting == false);

                topView.Show();
            }
        }

        private View GetTopView()
        {
            return _views.Count <= 0 ? null : _views.Peek();
        }
    }
}