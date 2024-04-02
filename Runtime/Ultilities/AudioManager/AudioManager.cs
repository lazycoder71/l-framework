using UnityEngine;
using UnityEngine.Pool;

namespace LFramework
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public static LValue<float> volumeMusic = new LValue<float>(1.0f);
        public static LValue<float> volumeSound = new LValue<float>(1.0f);

        ObjectPool<AudioScript> _pool;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();

            InitPool();
        }

        #endregion

        #region Public

        public static AudioScript Play(AudioConfig config, Vector3 pos, bool loop = false)
        {
            if (config.clip == null)
                return null;

            AudioScript audio = instance._pool.Get();
            audio.enabled = true;
            audio.Play(config, loop);
            audio.transformCached.position = pos;

            return audio;
        }

        public static AudioScript Play(AudioConfig config, bool loop = false)
        {
            return Play(config, Vector3.zero, loop);
        }

        public static void ReturnPool(AudioScript audioScript)
        {
            instance._pool.Release(audioScript);
        }

        #endregion

        void InitPool()
        {
            _pool = new ObjectPool<AudioScript>(() =>
            {
                return new GameObject(typeof(AudioScript).ToString(), typeof(AudioSource)).AddComponent<AudioScript>();
            }, (audio) =>
            {
                audio.gameObjectCached.SetActive(true);
            }, (audio) =>
            {
                audio.gameObjectCached.SetActive(false);
            }
            );
        }
    }
}