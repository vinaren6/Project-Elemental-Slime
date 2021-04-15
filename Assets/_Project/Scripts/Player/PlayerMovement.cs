using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _forward, _right;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _camera = Camera.main;
            SetIsometricReferences();
        }

        private void SetIsometricReferences()
        {
            _forward = _camera.transform.forward;
            _forward.y = 0;
            _forward = Vector3.Normalize(_forward);
            _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
        }

        public void Move(PlayerInputHandler inputHandler, float speed)
        {
            if (inputHandler.MoveDirection == Vector3.zero)
                return;

            var upMovement = _forward * inputHandler.MoveDirection.z;
            var rightMovement = _right * inputHandler.MoveDirection.x;
            var movementDirection = (upMovement + rightMovement).normalized;
            _rb.MovePosition(_rb.position + movementDirection * speed);
        }
    }
}