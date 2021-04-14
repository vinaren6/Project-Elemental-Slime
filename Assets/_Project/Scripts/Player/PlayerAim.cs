using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerAim : MonoBehaviour
    {
        private Plane _plane;

        private void Start()
        {
            _plane = new Plane(Vector3.up, Vector3.zero);
        }

        public void AimUpdate(Vector2 direction, Camera cameraRef)
        {
            Ray ray = cameraRef.ScreenPointToRay(direction);
            
            if (_plane.Raycast(ray, out float enter)) {
                Vector3 mousePosition = ray.GetPoint(enter);
                Vector3 playerPosition = _plane.ClosestPointOnPlane(transform.position);
                transform.rotation = Quaternion.LookRotation(mousePosition - playerPosition);
            }
        }
    }
}