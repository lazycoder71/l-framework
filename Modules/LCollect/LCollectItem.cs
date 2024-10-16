using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class LCollectItem : MonoBehaviour
    {
        Sequence _sequence;

        LCollectDestination _destination;

        RectTransform _rectTransform;

        public Sequence sequence { get { return _sequence; } }

        public LCollectDestination destination { get { return _destination; } }

        public RectTransform rectTransform { get { if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        public void Construct(LCollectConfig config, LCollectDestination destination)
        {
            if (destination == null)
            {
                Destruct();
                return;
            }

            _destination = destination;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            for (int i = 0; i < config.steps.Length; i++)
            {
                config.steps[i].Apply(this);
            }

            _sequence.OnComplete(() =>
            {
                _destination.Return();

                Destruct();
            });
        }

        private void Destruct()
        {
            Destroy(gameObject);
        }
    }
}
