using Sirenix.OdinInspector;
using UnityEngine;

namespace LFramework
{
    [System.Serializable]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] AudioClip _clip;
        [SerializeField] AudioType _type;
        [SerializeField] bool _is3D;

        [ShowIf("@_is3D")]
        [MinMaxSlider(0f, 200f, ShowFields = true)]
        [SerializeField] Vector2 _distance = new Vector2(1f, 10f);

        [Range(0f, 1f)]
        [SerializeField] float _volumeScale = 1f;

        public AudioClip clip { get { return _clip; } }
        public AudioType type { get { return _type; } }
        public bool is3D { get { return _is3D; } }
        public Vector2 distance { get { return _distance; } }
        public float volumeScale { get { return _volumeScale; } }

        public void Construct(AudioClip clip)
        {
            _clip = clip;
        }
    }
}