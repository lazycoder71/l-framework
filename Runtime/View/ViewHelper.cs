using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace LFramework.View
{
    public static class ViewHelper
    {
        public static UniTask<View> PushAsync(AssetReference viewAsset)
        {
            return ViewContainer.instance.PushAsync(viewAsset);
        }
    }
}
