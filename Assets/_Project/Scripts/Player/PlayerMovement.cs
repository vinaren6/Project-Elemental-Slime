using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Configuration")]
        [SerializeField] private float movementSpeed = 0.25f;
        
        private Rigidbody rb;
        private float horizontalInput;
        private float verticalInput;

        private Camera mainCamera;
        private Vector3 forward, right;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            mainCamera = Camera.main;
            SetIsometricReferences();
        }

        private void Update()
        {
            GetInput();
        }
        
        private void FixedUpdate()
        {
            MovePlayer();
        }
        
        private void SetIsometricReferences()
        {
            forward = mainCamera.transform.forward;
            forward.y = 0;
            forward = Vector3.Normalize(forward);
            right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        }

        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void MovePlayer()
        {
            if (horizontalInput == 0 && verticalInput == 0)
                return;
            
            Vector3 rightMovement = right * horizontalInput;
            Vector3 upMovement = forward * verticalInput;
            Vector3 movementDirection = Vector3.Normalize(rightMovement + upMovement);
            rb.MovePosition(rb.position + movementDirection * movementSpeed);
            // transform.forward = movementDirection; // See PlayerRotation
            
            // Smoother with Time.deltaTime according to Doc??
            // rb.MovePosition(rb.position + heading * (Time.deltaTime * movementSpeed));
        }
    }
}