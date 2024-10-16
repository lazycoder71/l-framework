using DG.Tweening;
using LFramework.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepAudio : LCollectStep
    {
        [HorizontalGroup]
        [SerializeField] private bool _isInserted;

        [HorizontalGroup]
        [Min(0f), ShowIf("@_isInserted")]
        [SerializeField]
        private float _insertTime;

        [SerializeField] private AudioConfig _audio;

        public override string displayName { get { return "Audio"; } }

        public override void Apply(LCollectItem item)
        {
            if (_isInserted)
                item.sequence.InsertCallback(_insertTime, () => { AudioManager.Play(_audio); });
            else
                item.sequence.AppendCallback(() => { AudioManager.Play(_audio); });
        }
    }
}