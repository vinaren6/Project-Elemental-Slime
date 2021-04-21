using System.Collections;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField][Range(1f,5f)] private float visibilityTime;
        [SerializeField][Range(0.5f, 3f)] private float fadeOutTime;

        private Image[]     _images;
        private HealthUI    _healthUI;
        private Coroutine _updateCoroutine;
        
        private void Awake()
        {
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            _images                                      = GetComponentsInChildren<Image>();
            _healthUI                                    = GetComponentInChildren<HealthUI>();

            SetImagesAlpha(0f);
        }

        private void LateUpdate()
        {
            transform.eulerAngles = new Vector3(0, 180 - targetTransform.rotation.y, 0);
        }

        [ContextMenu("ShowHealthBar")]
        public void ShowHealthBar(float remainingPercent)
        {
            _healthUI.UpdateHealthBar(remainingPercent);
            
            if (_updateCoroutine != null) {
                StopCoroutine(_updateCoroutine);
                _updateCoroutine = null;
            }
            _updateCoroutine = StartCoroutine(UpdateHealthBarRoutine());
        }

        private IEnumerator UpdateHealthBarRoutine()
        {
            float time = 0f;
            
            SetImagesAlpha(1f);

            yield return null;

            while (time < visibilityTime)
            {
                time += Time.deltaTime;
                
                yield return null;
            }

            time = 0f;

            while (time < fadeOutTime)
            {
                time += Time.deltaTime;
                
                float alpha = 1f - (time / fadeOutTime);
                
                SetImagesAlpha(alpha);
                
                yield return null;
            }
        }

        private void SetImagesAlpha(float alpha)
        {
            foreach (Image image in _images)
            {
                Color color = image.color;
                image.color = new Color(color.r, color.g, color.b, alpha);
            }
        }
    }
}
