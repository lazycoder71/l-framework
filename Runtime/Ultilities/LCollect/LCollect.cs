using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class LCollect : MonoCached
    {
        LCollectConfig _config;
        LCollectDestination _destination;

        Sequence _sequence;

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        public void Construct(LCollectConfig config, int valueCount)
        {
            _config = config;
            _destination = LCollectDestinationHelper.Get(_config);

            if (_destination == null)
            {
                LDebug.Log<LCollect>("Can't find destination");
                Destruct();
                return;
            }

            int spawnCount = config.GetSpawnCount(valueCount);

            _destination.ReturnBegin(valueCount, spawnCount);

            float delayBetween = spawnCount > 1 ? config.spawnDuration / (spawnCount - 1) : 0.0f;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = config.spawnPositions.GetLoop(i);

                _sequence.AppendCallback(() => { Spawn(spawnPosition); });
                _sequence.AppendInterval(delayBetween);
            }

            _sequence.Play();
            _sequence.OnComplete(StartCheckEmptyLoop);
        }

        private void StartCheckEmptyLoop()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(DOVirtual.DelayedCall(1.0f, null, false))
                     .SetLoops(-1, LoopType.Restart)
                     .OnUpdate(CheckEmpty);
        }

        private void CheckEmpty()
        {
            if (transformCached.childCount == 0)
                Destruct();
        }

        private void Destruct()
        {
            Destroy(gameObjectCached);
        }

        private void Spawn(Vector3 spawnPosition)
        {
            LCollectItem item = _config.spawnPrefab.Create(transformCached, false).GetComponent<LCollectItem>();

            item.transformCached.localPosition = spawnPosition;

            item.Construct(_config, _destination);
        }
    }
}
