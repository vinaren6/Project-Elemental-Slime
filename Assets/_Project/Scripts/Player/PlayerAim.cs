using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerAim : MonoBehaviour
    {
        private Camera _camera;
        private Plane  _plane;

        private void Start()
        {
            _camera = Camera.main;
            _plane  = new Plane(Vector3.up, Vector3.zero);
        }

        public void Aim(Vector2 direction)
        {
            Ray ray = _camera.ScreenPointToRay(direction);
            
            if (_plane.Raycast(ray, out float enter)) {
                Vector3 mousePosition = ray.GetPoint(enter);
                Vector3 playerPosition = _plane.ClosestPointOnPlane(transform.position);
                transform.rotation = Quaternion.LookRotation(mousePosition - playerPosition);
            }
        }
    }
}