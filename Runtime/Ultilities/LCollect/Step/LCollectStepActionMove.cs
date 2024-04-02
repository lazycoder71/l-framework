using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class LCollectStepActionMove : LCollectStepAction
    {
        [Serializable]
        public enum Journey
        {
            Spawn = 0,
            Return = 1,
        }

        [SerializeField] protected Journey _journey;

        [Space]

        [ShowIf("@_journey == Journey.Spawn")]
        [SerializeField] protected bool _startAtCenter;
        [ShowIf("@_journey == Journey.Spawn && !_startAtCenter")]
        [SerializeField] protected Vector3 _startOffset;
    }
}
