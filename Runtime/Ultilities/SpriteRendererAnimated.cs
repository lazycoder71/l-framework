﻿using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace LFramework
{
    public class SpriteRendererAnimated : MonoCached
    {
        [Header("Config")]
        [FormerlySerializedAs("_sprFrames")]
        [SerializeField] Sprite[] _frames;

        [Min(1)]
        [SerializeField] int _fps = 30;

        [SerializeField] int _loopCount = 0;

        [ShowIf("@_loopCount < 0")]
        [SerializeField] LoopType _loopType;

        SpriteRenderer _spriteRenderer;

        Sequence _sequence;

        public Sequence sequence
        {
            get
            {
                if (_sequence == null)
                    InitSequence();
                return _sequence;
            }
        }

        #region MonoBehaviour

        void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            InitSequence();
        }

        void OnDestroy()
        {
            _sequence?.Kill();
        }

        void OnEnable()
        {
            _sequence?.Restart();
            _sequence?.Play();
        }

        void OnDisable()
        {
            _sequence?.Pause();
        }

        #endregion

        void InitSequence()
        {
            if (_sequence != null)
                return;

            float delayBetween = 1.0f / _fps;

            _sequence = DOTween.Sequence();

            for (int i = 0; i < _frames.Length; i++)
            {
                int frameIndex = i;

                _sequence.AppendCallback(() => { SetFrame(frameIndex); });
                _sequence.AppendInterval(delayBetween);
            }

            _sequence.SetLoops(_loopCount, _loopType);
            _sequence.SetAutoKill(false);
        }

        void SetFrame(int frameIndex)
        {
            _spriteRenderer.sprite = _frames[frameIndex];
        }
    }
}