﻿using _Project.Scripts.ElementalSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
	public class PlayerElementalSystemSwitchElement : MonoBehaviour
	{
		public static ElementalSystemTypeCurrent StaticPlayerElementType;

		[SerializeField] private InputActionAsset           controls;
		[SerializeField] private ElementalSystemTypeCurrent elementType;
		[SerializeField] private int                        requiredElementsToChange;

		private readonly int[] _pickups = new int[5];


		private InputActionMap _inputActionMap;

		private void Awake()
		{
			StaticPlayerElementType = elementType;

			_inputActionMap = controls.FindActionMap("Element Controls");

			_inputActionMap.FindAction("Earth").performed += _ => Switch(0);
			_inputActionMap.FindAction("Wind").performed  += _ => Switch(1);
			_inputActionMap.FindAction("Water").performed += _ => Switch(2);
			_inputActionMap.FindAction("Fire").performed  += _ => Switch(3);
			_inputActionMap.FindAction("Base").performed  += _ => Switch(4);
		}

		private void OnEnable() => _inputActionMap.Enable();

		private void OnDisable() => _inputActionMap.Disable();

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Drop")) return;

			ElementalSystemTypeCurrent comp   = other.GetComponent<ElementalSystemTypeCurrent>();
			int                        typeId = (int) comp.Type;
			if (comp is ElementalSystemTypeCurrentFullPickup)
				_pickups[typeId] = requiredElementsToChange;
			else if (_pickups[typeId] < requiredElementsToChange)
				_pickups[typeId]++;
			comp.Destroy();
		}

		private void Switch(int type)
		{
			if (_pickups[type] < requiredElementsToChange && type != 4) return;
			_pickups[type]   -= requiredElementsToChange;
			elementType.Type =  (ElementalSystemTypes) type;
		}
	}
}