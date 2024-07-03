using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace LFramework.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class View : MonoCached
    {
        [Title("Config")]
        [MinValue(0f)]
        [SerializeField] private float _openDuration = 0.3f;

        [MinValue(0f)]
        [SerializeField] private float _closeDuration = 0.3f;

        [SerializeField] private ViewType _type = ViewType.Page;

        [FoldoutGroup("Extra", Expanded = false)]
        [ListDrawerSettings(ListElementLabelName = "displayName", AddCopiesLastElement = true)]
        [SerializeReference] private ViewExtra[] _extras = new ViewExtra[0];

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

        public ViewType type { get { return _type; } }

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

            // Active object
            gameObjectCached.SetActive(true);

            // Open start callback
            if (isShow)
                _onShowStart?.Invoke();
            else
                _onOpenStart?.Invoke();

            ConstructSequence();

            _canvasGroup.interactable = false;
            _isTransiting = true;

            if (_openDuration > 0.0f)
            {
                _sequence.timeScale = _openDuration > 0.0f ? 1.0f / _openDuration : 1.0f;

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

            // Close start callback
            if (isHiding)
                _onHideStart?.Invoke();
            else
                _onCloseStart?.Invoke();

            ConstructSequence();

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

                Destroy(gameObjectCached);
            }
        }

        #endregion

        #region Function -> Public

        public async UniTask Open()
        {
            await ProcessOpen(true);
        }

        public void Close()
        {
            ProcessClose(false);
        }

        public void Show()
        {
            ProcessOpen(true);
        }

        public void Hide()
        {
            ProcessClose(true);
        }

        #endregion
    }
}