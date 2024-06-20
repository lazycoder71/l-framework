using System;

namespace LFramework
{
    [Serializable]
    public abstract class AnimationSequenceStep
    {
        [Serializable]
        public enum AddType
        {
            Append = 0,
            Join = 1,
            Insert = 2,
        }

        public abstract string displayName { get; }

        public abstract void AddToSequence(AnimationSequence animationSequence);
    }
}