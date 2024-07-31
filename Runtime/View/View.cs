using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace LFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class View : MonoCached
    {
        [Title("Config")]
        [MinValue(0f)]
        [SerializeField] private float _openDuration = 0.3f;

        [MinValue(0f)]
        [SerializeField] private float _closeDuration = 0.3f;

        [FoldoutGroup("Extra", Expanded = false)]
        [ListDrawerSettings(ListElementLabelName = "displayName")]
        [SerializeReference] private ViewExtra[] _extras = new ViewExtra[0];

        [FoldoutGroup("Extra")]
        [SerializeField] private bool _hideOnBlock = false;
        [SerializeField] private bool _showOnReveal = false;

        [FoldoutGroup("Transition", Expanded = false)]
        [SerializeField] private ViewTransitionEntity[] _transitionEntities;

        [FoldoutGroup("Event", Expanded = false)]
        [SerializeField] private UnityEvent _onOpenStart;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onOpenEnd;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onCloseStart;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onCloseEnd;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onShowStart;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onShowEnd;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onHideStart;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onHideEnd;

        private Sequence _sequence;

        private CanvasGroup _canvasGroup;

        private bool _isTransiting = false;

        public bool isTransiting { get { return _isTransiting; } }

        public Sequence sequence { get { return _sequence; } }

        public UnityEvent onOpenStart { get { return _onOpenStart; } }
        public UnityEvent onOpenEnd { get { return _onOpenEnd; } }
        public UnityEvent onCloseStart { get { return _onCloseStart; } }
        public UnityEvent onCloseEnd { get { return _onCloseEnd; } }
        public UnityEvent onShowStart { get { return _onShowStart; } }
        public UnityEvent onShowEnd { get { return _onShowEnd; } }
        public UnityEvent onHideStart { get { return _onHideStart; } }
        public UnityEvent onHideEnd { get { return _onHideEnd; } }

        public bool interactable { get { return _canvasGroup.interactable; } set { _canvasGroup.interactable = value; } }

        public bool hideOnBlock { get { return _hideOnBlock; } }
        public bool showOnReveal { get { return _showOnReveal; } }

        #region MonoBehaviour

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDestroy()
        {
            // Kill tweens
            _sequence?.Kill();
        }

        #endregion

        #region Function -> Private

        [FoldoutGroup("Transition")]
        [Button]
        private void GetTransitionEntities()
        {
            _transitionEntities = GetComponentsInChildren<ViewTransitionEntity>(true);
        }

        private void ConstructSequence()
        {
            if (_sequence != null)
                return;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            if (_extras.IsNullOrEmpty() && _transitionEntities.IsNullOrEmpty())
            {
                _sequence.AppendInterval(1.0f);
            }
            else
            {
                for (int i = 0; i < _extras.Length; i++)
                    _extras[i].Apply(this);

                for (int i = 0; i < _transitionEntities.Length; i++)
                    _transitionEntities[i].Apply(this);
            }

            _sequence.SetUpdate(true);
            _sequence.SetAutoKill(false);
        }

        private async UniTask ProcessOpen(bool isShow)
        {
            // Can't open when it is transiting
            if (_isTransiting)
                return;

            _isTransiting = true;

            gameObjectCached.SetActive(false);

            ConstructSequence();

            // Open start callback
            if (isShow)
                _onShowStart?.Invoke();
            else
                _onOpenStart?.Invoke();

            // Active object
            gameObjectCached.SetActive(true);

            _canvasGroup.interactable = false;

            if (_openDuration > 0.0f)
            {
                _sequence.timeScale = _openDuration > 0.0f ? 1.0f / _openDuration : 1.0f;

                _sequence.Restart();

                await _sequence.Play().AsyncWaitForCompletion().AsUniTask();
            }
            else
            {
                _sequence.Complete();
            }

            // Open end callback
            if (isShow)
                _onShowEnd?.Invoke();
            else
                _onOpenEnd?.Invoke();

            _canvasGroup.interactable = true;
            _isTransiting = false;
        }

        private async UniTask ProcessClose(bool isHiding)
        {
            // Can't close when it is transiting
            if (_isTransiting)
                return;

            ConstructSequence();

            // Close start callback
            if (isHiding)
                _onHideStart?.Invoke();
            else
                _onCloseStart?.Invoke();

            _isTransiting = true;
            _canvasGroup.interactable = false;

            if (_closeDuration > 0.0f)
            {
                _sequence.timeScale = _closeDuration > 0.0f ? 1.0f / _closeDuration : 1.0f;
                _sequence.PlayBackwards();

                await _sequence.AsyncWaitForRewind().AsUniTask();
            }
            else
            {
                _sequence.Rewind();
            }

            if (isHiding)
            {
                _onHideEnd?.Invoke();

                gameObjectCached.SetActive(false);
            }
            else
            {
                _onCloseEnd?.Invoke();
            }

            _isTransiting = false;
        }

        #endregion

        #region Function -> Public

        public UniTask OpenAsync()
        {
            return ProcessOpen(false);
        }

        public void Open()
        {
            ProcessOpen(false).Forget();
        }

        public UniTask CloseAsync()
        {
            return ProcessClose(false);
        }

        public void Close()
        {
            ProcessClose(false).Forget();
        }

        public UniTask ShowAsync()
        {
            return ProcessOpen(true);
        }

        public void Show()
        {
            ProcessOpen(true).Forget();
        }

        public UniTask HideAsync()
        {
            return ProcessClose(true);
        }

        public void Hide()
        {
            ProcessClose(true).Forget();
        }

        #endregion
    }
}