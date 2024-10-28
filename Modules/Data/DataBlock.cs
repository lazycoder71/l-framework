using System;

namespace LFramework.Data
{
    public class DataBlock<T> where T : DataBlock<T>
    {
        static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = DataHelper.LoadFromDevice<T>(typeof(T).ToString());

                    if (s_instance == null)
                        s_instance = (T)Activator.CreateInstance(typeof(T));

                    s_instance.Init();
                }

                return s_instance;
            }
        }

        protected virtual void Init()
        {
            MonoCallback.Instance.EventApplicationPause += MonoCallback_ApplicationOnPause;
            MonoCallback.Instance.EventApplicationQuit += MonoCallback_ApplicationOnQuit;
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
            DataHelper.SaveToDevice(Instance, typeof(T).ToString());
        }

        public static void Delete()
        {
            s_instance = null;

            DataHelper.DeleteInDevice(typeof(T).ToString());
        }
    }
}