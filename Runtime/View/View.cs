using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;

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

        [Space]

        [FoldoutGroup("Extra", Expanded = false)]
        [ListDrawerSettings(ListElementLabelName = "displayName", AddCopiesLastElement = true)]
        [SerializeReference] ViewExtra[] _extras = new ViewExtra[0];

        [Space]

        [FoldoutGroup("Event", Expanded = false)]
        [SerializeField] private UnityEvent _onOpenStart;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onOpenEnd;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onCloseStart;
        [FoldoutGroup("Event")]
        [SerializeField] private UnityEvent _onCloseEnd;

        private Sequence _sequence;

        private CanvasGroup _canvasGroup;

        private bool _isTransiting = false;

        public Sequence sequence { get { return _sequence; } }

        public float openDuration { get { return _openDuration; } }
        public float closeDuration { get { return _closeDuration; } }

        public UnityEvent onOpenStart { get { return _onOpenStart; } }
        public UnityEvent onOpenEnd { get { return _onOpenEnd; } }
        public UnityEvent onCloseStart { get { return _onCloseStart; } }
        public UnityEvent onCloseEnd { get { return _onCloseEnd; } }

        public bool interactable { get { return _canvasGroup.interactable; } set { _canvasGroup.interactable = value; } }

        #region MonoBehaviour

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void OnDestroy()
        {
            // Kill tweens
            _sequence?.Kill();
        }

        private void OnEnable()
        {
            _onOpenStart?.Invoke();

            // Construct sequence and play it
            ConstructSequence();

            _sequence.Restart();
            _sequence.Play();
        }

        private void OnDisable()
        {
            // Trigger close end event
            _onCloseEnd?.Invoke();
        }

        #endregion

        #region Function -> Public

        public void Close()
        {
            ProcessClose();
        }

        #endregion

        #region Function -> Private

        private void ConstructSequence()
        {
            if (_sequence != null)
                return;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            if (_extras != null)
            {
                for (int i = 0; i < _extras.Length; i++)
                    _extras[i].Apply(this);
            }

            _sequence.SetUpdate(true);
            _sequence.SetAutoKill(false);
        }

        private void ProcessClose()
        {
            // Can't close when it is transiting
            if (_isTransiting)
                return;

            // Set is transiting flag
            _isTransiting = true;

            // Disable canvas at this moment
            _canvasGroup.interactable = false;

            // On close callback
            _onCloseStart?.Invoke();

            if (_closeDuration > 0.0f)
            {
                // Set sequence time scale to match close duration
                _sequence.timeScale = _openDuration / _closeDuration;

                // Play sequence backward when close
                _sequence.Complete();
                _sequence.PlayBackwards();
            }
            else
            {

            }
        }

        #endregion
    }
}