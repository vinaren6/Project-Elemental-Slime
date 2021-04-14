using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
	public class PlayerInputHandler : MonoBehaviour
	{
		private PlayerInput _gameplay;
		
		public Vector3 MoveDirection { get; private set; }
		public Vector2 AimDirection  { get; private set; }
		public bool    AttackInput        { get; private set; }
		public bool    AttackInputStop    { get; private set; }

		private void Awake()
		{
			SetupInputListeners();
		}

		private void OnMoveInput(InputAction.CallbackContext moveAction)
		{
			Vector2 moveDirection = moveAction.ReadValue<Vector2>();
			MoveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
		}

		public void OnAttackInput(InputAction.CallbackContext attackAction)
		{
			if (attackAction.started) {
				AttackInput     = true;
				AttackInputStop = false;
			}

			if (attackAction.canceled) { AttackInputStop = true; }
		}

		public void UseAttackInput() => AttackInput = false;
		
		private void SetupInputListeners()
		{
			_gameplay                                      =  GetComponent<PlayerInput>();
			
			_gameplay.actions.FindAction("Move").performed += OnMoveInput;
			_gameplay.actions.FindAction("Move").canceled  += OnMoveInput;
			_gameplay.actions.FindAction("Aim").performed  += ctx => AimDirection = ctx.ReadValue<Vector2>();
			_gameplay.actions.FindAction("Aim").canceled   += ctx => AimDirection = ctx.ReadValue<Vector2>();
		}
		
		private void OnEnable()
		{
			_gameplay.actions.Enable();
		}
		
		private void OnDisable()
		{
			_gameplay.actions.Disable();
		}
	}
}