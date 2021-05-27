using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerAim : MonoBehaviour
    {
        private PlayerController _player;
        private Camera           _camera;
        private Plane            _plane;
        private Transform        _transform;

        private void Start()
        {
            _player    = GetComponent<PlayerController>();
            _camera    = Camera.main;
            _plane     = new Plane(Vector3.up, Vector3.zero);
            _transform = gameObject.transform;
        }

        public void Aim(Vector2 direction)
        {
            Ray ray = _camera.ScreenPointToRay(direction);
            
            if (_plane.Raycast(ray, out float enter)) {
                Vector3    mousePosition  = ray.GetPoint(enter);
                Vector3    playerPosition = _plane.ClosestPointOnPlane(_transform.position);
                Quaternion target         = Quaternion.LookRotation(mousePosition - playerPosition);
                transform.rotation = target;
            }
        }
    }
}