using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace LFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Popup : MonoCached
    {
        [Title("Config")]
        [SerializeField, Min(0.01f)] float _openDuration = 0.2f;
        [SerializeField, Min(0.01f)] float _closeDuration = 0.2f;
        [SerializeField] bool _isLocked = false;
        [SerializeField] bool _deactiveOnClosed = false;

        [Space]

        [FoldoutGroup("Animation", Expanded = false)]
        [ListDrawerSettings(ListElementLabelName = "displayName", AddCopiesLastElement = true)]
        [SerializeReference] PopupAnimation[] _animations = new PopupAnimation[0];

        [Space]

        [FoldoutGroup("Event", Expanded = false)]
        [SerializeField] UnityEvent _onOpenStart;
        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onOpenEnd;
        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onCloseStart;
        [FoldoutGroup("Event")]
        [SerializeField] UnityEvent _onCloseEnd;

        bool _isOpening = false;
        bool _isEnabled = false;

        Sequence _sequence;

        CanvasGroup _canvasGroup;

        public float openDuration { get { return _openDuration; } }
        public float closeDuration { get { return _closeDuration; } }
        public bool isLocked { get { return _isLocked; } set { _isLocked = value; } }
        public bool deactiveOnClosed { get { return _deactiveOnClosed; } }
        public CanvasGroup canvasGroup { get { return _canvasGroup; } }
        public Sequence sequence { get { return _sequence; } }

        public UnityEvent onOpenStart { get { return _onOpenStart; } }
        public UnityEvent onOpenEnd { get { return _onOpenEnd; } }
        public UnityEvent onCloseStart { get { return _onCloseStart; } }
        public UnityEvent onCloseEnd { get { return _onCloseEnd; } }

        #region MonoBehaviour

        void Awake()
        {
            // Get canvas group component and disable UI constrol at begin
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void OnDestroy()
        {
            // Kill tweens
            _sequence?.Kill();
        }

        private void Update()
        {
            if (!_isLocked && _isEnabled)
                InputCheck();
        }

        private void OnEnable()
        {
            // Add this popup into stack
            PopupManager.PushToStack(this);

            _isOpening = true;

            _isEnabled = true;

            _onOpenStart?.Invoke();

            // Construct sequence and play it
            ConstructSequence();

            _sequence.Restart();
            _sequence.Play();
        }

        private void OnDisable()
        {
            // Pop this popup out of stack
            PopupManager.PopFromStack(this);

            // Trigger close end event
            _onCloseEnd?.Invoke();
        }

        #endregion

        #region Function -> Public

        public void Close()
        {
            if (_isLocked)
                return;

            ProcessClose();
        }

        public void CloseForced()
        {
            ProcessClose();
        }

        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
        }

        #endregion

        #region Function -> Private

        private void ConstructSequence()
        {
            if (_sequence != null)
                return;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            if (!_animations.IsNullOrEmpty())
            {
                for (int i = 0; i < _animations.Length; i++)
                    _animations[i].Apply(this, _sequence);
            }
            else
            {
                _sequence.AppendInterval(Mathf.Max(_openDuration, 0.05f));
            }

            _sequence.OnStepComplete(Sequence_OnStepComplete);
            _sequence.SetUpdate(true);
            _sequence.SetAutoKill(false);
        }

        private void Sequence_OnStepComplete()
        {
            if (_isOpening)
            {
                _onOpenEnd?.Invoke();

                _canvasGroup.interactable = true;
            }
            else
            {
                if (_deactiveOnClosed)
                    gameObjectCached.SetActive(false);
                else
                    Destroy(gameObjectCached);
            }
        }

        private void ProcessClose()
        {
            // Can't close when it is transiting
            if (_sequence.IsPlaying())
                return;

            // Set is opening flag
            _isOpening = false;

            // Disable canvas at this moment
            _canvasGroup.interactable = false;

            // On close callback
            _onCloseStart?.Invoke();

            if (_openDuration > 0.0f && _closeDuration > 0.0f)
            {
                // Set sequence time scale to match close duration
                _sequence.timeScale = _openDuration / _closeDuration;
            }

            // Play sequence backward when close
            _sequence.PlayBackwards();
        }

        private void InputCheck()
        {
#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
#endif
        }

        [Button]
        private void SetupRectTransform()
        {
            RectTransform rect = GetComponent<RectTransform>();

            rect.StretchByParent();
        }

        #endregion
    }
}