using System;
using System.Collections.Generic;
using UnityEngine;

namespace LFramework.Audio
{
    public static class AudioManager
    {
        public static LValue<float> VolumeMaster = new LValue<float>(1.0f);
        public static LValue<float> VolumeMusic = new LValue<float>(1.0f);
        public static LValue<float> VolumeSound = new LValue<float>(1.0f);

        private static List<AudioEntity> s_entities = new List<AudioEntity>();

        [RuntimeInitializeOnLoadMethod]
        private static void InitOnStartup()
        {
            VolumeMaster.EventValueChanged += Volume_EventValueChanged;
            VolumeMusic.EventValueChanged += Volume_EventValueChanged;
            VolumeSound.EventValueChanged += Volume_EventValueChanged;
        }

        #region Function -> Private

        private static void Volume_EventValueChanged(float volume)
        {
            for (int i = 0; i < s_entities.Count; i++)
            {
                s_entities[i].UpdateVolume();
            }
        }

        #endregion

        #region Function -> Public

        public static void Register(AudioEntity entity)
        {
            s_entities.Add(entity);
        }

        public static void Unregister(AudioEntity entity)
        {
            s_entities.Remove(entity);
        }

        public static AudioEntity Play(AudioConfig config, bool isLoop = false)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            AudioEntity audioEntity = AudioEntityPool.Get();

            audioEntity.Play(config, isLoop: isLoop);

            return audioEntity;
        }

        public static void Stop(AudioConfig config, bool isAll = false)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            for (int i = 0; i < s_entities.Count; i++)
            {
                if (s_entities[i].Config == config)
                {
                    s_entities[i].Stop();

                    if (!isAll)
                        break;
                }
            }
        }

        public static void StopAll()
        {
            for (int i = 0; i < s_entities.Count; i++)
            {
                s_entities[i].Stop();
            }
        }

        public static AudioEntity Find(AudioConfig config)
        {
            for (int i = 0; i < s_entities.Count; i++)
            {
                if (s_entities[i].Config == config)
                {
                    return s_entities[i];
                }
            }

            return null;
        }

        #endregion
    }
}