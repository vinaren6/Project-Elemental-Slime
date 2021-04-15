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
		private PlayerController _playerController;
		
		public Vector3 MoveDirection { get; private set; }
		public Vector2 AimDirection  { get; private set; }
		public bool    FireInput        { get; private set; }

		private void Awake()
		{
			SetupInputListeners();
		}

		private void OnMoveInput(InputAction.CallbackContext moveAction)
		{
			Vector2 moveDirection = moveAction.ReadValue<Vector2>();
			MoveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
		}

		private void OnFireInput(InputAction.CallbackContext fireAction)
		{
			if (fireAction.started) { FireInput = true; }
			if (fireAction.canceled) { FireInput = false; }
		}

		private void SetupInputListeners()
		{
			_gameplay                                      =  GetComponent<PlayerInput>();
			_playerController = GetComponent<PlayerController>();
			
			_gameplay.actions.FindAction("Move").performed += OnMoveInput; 
			_gameplay.actions.FindAction("Move").canceled  += OnMoveInput;
			_gameplay.actions.FindAction("Aim").performed  += ctx => AimDirection = ctx.ReadValue<Vector2>();
			_gameplay.actions.FindAction("Aim").canceled   += ctx => AimDirection = ctx.ReadValue<Vector2>();
			_gameplay.actions.FindAction("Fire").started += OnFireInput;
			_gameplay.actions.FindAction("Fire").canceled  += OnFireInput;
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