using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepAudio : LCollectStep
    {
        [SerializeField, HorizontalGroup]
        private bool _isInserted;

        [HorizontalGroup]
        [SerializeField, Min(0f), ShowIf("@_isInserted")]
        private float _insertTime;

        [SerializeField]
        private AudioConfig _audio;

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
