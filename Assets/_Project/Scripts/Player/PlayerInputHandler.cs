using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
	public class PlayerInputHandler : MonoBehaviour
	{
		public int  HorizontalInput { get; private set; }
		public int  VerticalInput   { get; private set; }
		public bool AttackInput     { get; private set; }
		public bool AttackInputStop { get; private set; }
		
		public void OnMoveInput(InputAction.CallbackContext moveAction)
		{
			if (moveAction.started) { HorizontalInput = (int) moveAction.ReadValue<float>(); }

			if (moveAction.canceled) { HorizontalInput = (int) moveAction.ReadValue<float>(); }
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
	}
}