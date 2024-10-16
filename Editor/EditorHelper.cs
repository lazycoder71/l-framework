using UnityEditor;

namespace LFramework.Editor
{
    public static class EditorHelper
    {
        [MenuItem("LFramework/Clear Device Data")]
        private static void ClearDeviceData()
        {
            LDataBlockHelper.ClearDeviceData();
        }
    }
}
