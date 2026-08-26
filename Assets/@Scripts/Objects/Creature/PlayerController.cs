using UnityEngine;
using static Define;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(PlayerWeapon))]
public class PlayerController : CreatureObject
{
	[SerializeField] private float _moveSpeed = 6f;
	[SerializeField] private float _turnSpeed = 720f;

	private Rigidbody _rb;
	private PlayerInputReader _input;
	private PlayerWeapon _weapon;
	private Camera _cam;

	private Vector3 _aimDirection = Vector3.forward;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		CreatureType = ECharacterType.Player;

		_rb = GetComponent<Rigidbody>();
		_rb.freezeRotation = true;
		_rb.useGravity = false;

		_input = GetComponent<PlayerInputReader>();
		_weapon = GetComponent<PlayerWeapon>();
		_cam = Camera.main;

		return true;
	}

	private void FixedUpdate()
	{
		if (IsDead)
			return;

		Vector3 move = new Vector3(_input.MoveInput.x, 0f, _input.MoveInput.y);
		_rb.MovePosition(_rb.position + move * _moveSpeed * Time.fixedDeltaTime);

		SetState(move.sqrMagnitude > 0.0001f ? ECharacterState.Move : ECharacterState.Idle);
	}

	private void Update()
	{
		if (IsDead)
			return;

		AimAtMouseGround();
		_weapon.SetFiring(_input.AttackHeld, _aimDirection);
	}

	private void AimAtMouseGround()
	{
		if (_cam == null)
			return;

		Ray ray = _cam.ScreenPointToRay(_input.AimScreenPosition);
		Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

		if (groundPlane.Raycast(ray, out float enter))
		{
			Vector3 point = ray.GetPoint(enter);
			Vector3 dir = point - transform.position;
			dir.y = 0f;

			if (dir.sqrMagnitude > 0.0001f)
			{
				_aimDirection = dir.normalized;
				Quaternion targetRot = Quaternion.LookRotation(_aimDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, _turnSpeed * Time.deltaTime);
			}
		}
	}

	protected override void Die()
	{
		base.Die();

		_rb.linearVelocity = Vector3.zero;
		Managers.Game.SetState(EGameState.GameOver);
	}
}
