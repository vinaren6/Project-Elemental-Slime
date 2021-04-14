using System;
using _Project.Scripts.ElementalSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
	public class PlayerElementalSystemSwitchElement : MonoBehaviour
	{
		[SerializeField] private InputActionAsset           controls;
		[SerializeField] private ElementalSystemTypeCurrent elementType;
		[SerializeField] private int                        requiredElementsToChange;
		
		
		private InputActionMap _inputActionMap;

		private readonly int[] _pickups = new int[5];

		private void Awake()
		{
			_inputActionMap = controls.FindActionMap("Element Controls");
			
			_inputActionMap.FindAction("Earth").performed += _ => Switch(0);
			_inputActionMap.FindAction("Wind").performed  += _ => Switch(1);
			_inputActionMap.FindAction("Water").performed += _ => Switch(2);
			_inputActionMap.FindAction("Fire").performed  += _ => Switch(3);
			_inputActionMap.FindAction("Base").performed  += _ => Switch(4);
		}

		private void OnEnable() => _inputActionMap.Enable();

		private void OnDisable() => _inputActionMap.Disable();

		private void Switch(int type)
		{
			if (_pickups[type] < requiredElementsToChange && type != 5) return;
			_pickups[type]   -= requiredElementsToChange;
			elementType.Type =  (ElementalSystemTypes) type;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Drop"))
				_pickups[(int) other.GetComponent<ElementalSystemTypeCurrent>().Type]++;
		}
	}
}