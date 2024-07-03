using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LFramework.View
{
    public class ViewContainer : MonoSingleton<ViewContainer>
    {
        private Stack<View> _views = new Stack<View>();

        private bool _isTransiting = false;

        private View _topView { get { return _views.Count <= 0 ? null : _views.Peek(); } }

        public async UniTask<View> PushAsync(AssetReference viewAsset)
        {
            // Can't push another view when it is transiting
            if (_isTransiting)
            {
                LDebug.Log<ViewContainer>($"Views are transiting, can't push new view {viewAsset}");
                return null;
            }

            // Get top view
            View topView = _topView;

            // Disable interactable top view
            if (topView != null)
                topView.interactable = false;

            _isTransiting = true;

            // Spawn view
            View view = (await viewAsset.InstantiateAsync(transformCached, false).Task.AsUniTask()).GetComponent<View>();

            await view.Open();

            _isTransiting = false;

            if (topView != null && view.type == ViewType.Page)
                topView.Hide();

            return view;
        }

        public async UniTaskVoid PopAsync(View view)
        {
            View popedView = _views.Pop();

            View topView = _topView;

            if (topView == null)
                return;

            topView.interactable = true;

            if (popedView.type == ViewType.Page)
            {
                await UniTask.WaitUntil(() => topView.isTransiting == false);

                topView.Show();
            }

            topView.Show();
        }
    }
}