using System;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        private void Awake()
        {
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.eulerAngles = new Vector3(0, 180 - targetTransform.rotation.y, 0);
        }
    }
}
