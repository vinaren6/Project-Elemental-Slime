using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        private Camera mainCamera;
        private Plane plane;

        private void Start()
        {
            mainCamera = Camera.main;
            plane = new Plane(Vector3.up, Vector3.zero);
        }

        private void Update()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float enter)) 
            {
                Vector3 mousePosition = ray.GetPoint(enter);
                Vector3 playerPosition = plane.ClosestPointOnPlane(transform.position);
                transform.rotation = Quaternion.LookRotation(mousePosition - playerPosition);
            }
        }
    }
}