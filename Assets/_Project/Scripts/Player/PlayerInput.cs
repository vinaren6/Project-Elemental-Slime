using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
	public class PlayerInput : MonoBehaviour
	{
		private UnityEngine.InputSystem.PlayerInput _gameplay;

		public Vector3 MoveDirection { get; private set; }
		public Vector2 AimDirection  { get; private set; }
		public bool    FireInput     { get; private set; }

		private void Awake() => SetupInputListeners();

		private void OnEnable() => _gameplay.actions.Enable();

		private void OnDisable() => _gameplay.actions.Disable();

		private void OnMoveInput(InputAction.CallbackContext moveAction)
		{
			var moveDirection = moveAction.ReadValue<Vector2>();
			MoveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
		}

		private void OnFireInput(InputAction.CallbackContext fireAction)
		{
			if (fireAction.started)
				FireInput = true;
			else if (fireAction.canceled)
				FireInput = false;
		}

		private void SetupInputListeners()
		{
			_gameplay = GetComponent<UnityEngine.InputSystem.PlayerInput>();

			_gameplay.actions.FindAction("Move").performed += OnMoveInput;
			_gameplay.actions.FindAction("Move").canceled  += OnMoveInput;
			_gameplay.actions.FindAction("Aim").performed  += ctx => AimDirection = ctx.ReadValue<Vector2>();
			_gameplay.actions.FindAction("Aim").canceled   += ctx => AimDirection = ctx.ReadValue<Vector2>();
			_gameplay.actions.FindAction("Fire").started   += OnFireInput;
			_gameplay.actions.FindAction("Fire").canceled  += OnFireInput;
		}
	}
}