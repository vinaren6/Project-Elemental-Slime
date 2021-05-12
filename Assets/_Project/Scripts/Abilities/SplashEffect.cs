using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class SplashEffect : MonoBehaviour
    {
        private Transform _transform;
        private Transform _originalParent;
        private ParticleSystem _particleSystem;

        private float _timer;
        private float _splashTime;

        private void Awake()
        {
            _transform = transform;
            _originalParent = _transform.parent;
            _particleSystem = GetComponent<ParticleSystem>();

            _splashTime = _particleSystem.main.duration;
        }

        public void StartEffect(Transform parent)
        {
            _transform.SetParent(parent);
            
            _transform.localPosition = Vector3.zero;
            float rotY = _transform.eulerAngles.y - 180f;
            _transform.localRotation = Quaternion.Euler(0f, rotY, 0f);

            _timer = 0f;

            if (_particleSystem.isPlaying)
                return;
            
            StartCoroutine(SplashRoutine());
        }

        public void StopEffect()
        {
            _transform.SetParent(_originalParent);
            _particleSystem.Stop();
            
            // TODO DENNA FUNKAR BARA OM VI INTE FÖRSTÖR DET MAN SKJUTER PÅ.
            // TODO DEN TAPPAR REFERENS ISF!!!!!!!! DELAY PÅ DESTROY I VÄRSTA FALL???? PLEASE?!!?
        }

        private IEnumerator SplashRoutine()
        {
            _particleSystem.Play();

            while (_timer < _splashTime)
            {
                yield return null;
                _timer += Time.deltaTime;
            }

            StopEffect();
        }
    }
}
