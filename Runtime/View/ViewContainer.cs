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

        public async UniTask<View> PushAsync(AssetReference viewAsset)
        {
            // Can't push a view when it is transiting
            if (_isTransiting)
                return null;

            // Disable interactable last view
            if (_views.Count > 0)
                _views.Peek().interactable = false;

            _isTransiting = true;

            GameObject objView = await viewAsset.InstantiateAsync(transformCached, false).Task.AsUniTask();

            _isTransiting = false;

            return objView.GetComponent<View>();
        }
    }
}