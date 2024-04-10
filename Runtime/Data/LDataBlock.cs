using System;
using System.IO;

namespace LFramework
{
    public class LDataBlock<T> where T : LDataBlock<T>
    {
        static T s_instance;

        public static T instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = LDataHelper.LoadFromDevice<T>(typeof(T).ToString());

                    if (s_instance == null)
                        s_instance = (T)Activator.CreateInstance(typeof(T));

                    s_instance.Init();
                }

                return s_instance;
            }
        }

        protected virtual void Init()
        {
            MonoCallback.instance.eventApplicationPause += MonoCallback_ApplicationOnPause;
            MonoCallback.instance.eventApplicationQuit += MonoCallback_ApplicationOnQuit;

            LDataBlockHelper.eventDelete += LDataBlockHelper_EventDelete;
        }

        private void MonoCallback_ApplicationOnQuit()
        {
            Save();
        }

        private void MonoCallback_ApplicationOnPause(bool paused)
        {
            if (paused)
                Save();
        }

        private void LDataBlockHelper_EventDelete()
        {
            s_instance = null;
        }

        public static void Save()
        {
            LDataHelper.SaveToDevice(instance, typeof(T).ToString());
        }

        public static void Delete()
        {
            s_instance = null;

            LDataHelper.DeleteInDevice(typeof(T).ToString());
        }
    }

    public class LDataBlockHelper
    {
        public static event Action eventDelete;

        public static void ClearDeviceData()
        {
            eventDelete?.Invoke();

            LDataHelper.DeleteAllInDevice();
        }
    }
}