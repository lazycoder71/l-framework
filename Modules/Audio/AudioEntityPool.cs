using UnityEngine;
using UnityEngine.Pool;

namespace LFramework.Audio
{
    public class AudioEntityPool
    {
        private static ObjectPool<AudioEntity> s_pool;

        #region Function -> Public

        public static void Release(AudioEntity audioScript)
        {
            s_pool.Release(audioScript);
        }

        public static AudioEntity Get()
        {
            InitPool();

            AudioEntity audio = null;

            while (audio == null)
                audio = s_pool.Get();

            return audio;
        }

        #endregion

        #region Function -> Private

        private static void InitPool()
        {
            if (s_pool != null)
                return;

            s_pool = new ObjectPool<AudioEntity>(
                () =>
                {
                    // Create object with audio script + audio source
                    AudioEntity audio = new GameObject(typeof(AudioEntity).ToString(), typeof(AudioSource)).AddComponent<AudioEntity>();

                    // Audio should not stop when scene changed
                    Object.DontDestroyOnLoad(audio.GameObjectCached);

                    return audio;
                },
                (audio) => { audio.GameObjectCached.SetActive(true); },
                (audio) => { audio.GameObjectCached.SetActive(false); },
                (audio) => { },
#if UNITY_EDITOR
                true); // Keep heavy check on editor
#else
                false);
#endif
        }

        #endregion
    }
}