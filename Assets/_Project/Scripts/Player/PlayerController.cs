using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("PLAYER SETTINGS:")]
		[SerializeField] private float moveSpeed   = 10f;
		[SerializeField] private float attackRate      = 0.5f;
		[SerializeField] private float projectileSpeed = 1f;
		
		private PlayerInput _input;
		private PlayerAim   _aim;
		private PlayerMove  _move;
		private PlayerShoot _shoot;
		
		private Camera _camera;

		private void Awake()
		{
			_input = GetComponent<PlayerInput>();
			_aim   = GetComponent<PlayerAim>();
			_move  = GetComponent<PlayerMove>();
			_shoot = GetComponent<PlayerShoot>();
		}

		private void Start()
		{
			_camera = Camera.main;
		}

		private void Update()
		{
			_aim.Aim(_input.AimDirection, _camera);
			if (_input.FireInput)
				_shoot.Fire(attackRate, projectileSpeed);
		}

		private void FixedUpdate()
		{
			_move.Move(_input.MoveDirection, moveSpeed);
		}
	}
}