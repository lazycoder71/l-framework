using System;

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
        }

        protected virtual void OnSaved()
        {

        }

        protected virtual void OnDeleted()
        {

        }

        void MonoCallback_ApplicationOnQuit()
        {
            Save();
        }

        void MonoCallback_ApplicationOnPause(bool paused)
        {
            if (paused)
                Save();
        }

        public static void Save()
        {
            instance.OnSaved();

            LDataHelper.SaveToDevice(instance, typeof(T).ToString());
        }

        public static void Delete()
        {
            instance.OnDeleted();

            s_instance = null;

            LDataHelper.DeleteFromDevice(typeof(T).ToString());
        }
    }
}