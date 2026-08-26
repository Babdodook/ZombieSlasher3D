using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
	private InputSystem_Actions _actions;

	public Vector2 MoveInput { get; private set; }
	public Vector2 AimScreenPosition { get; private set; }
	public bool AttackHeld { get; private set; }

	private void Awake()
	{
		_actions = new InputSystem_Actions();
	}

	private void OnEnable()
	{
		_actions.Player.Enable();
	}

	private void OnDisable()
	{
		_actions.Player.Disable();
	}

	private void Update()
	{
		MoveInput = _actions.Player.Move.ReadValue<Vector2>();
		AimScreenPosition = _actions.Player.AimPosition.ReadValue<Vector2>();
		AttackHeld = _actions.Player.Attack.IsPressed();
	}
}
