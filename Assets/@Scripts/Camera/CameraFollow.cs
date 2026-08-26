using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private Vector3 _offset = new Vector3(0f, 12f, -8f);
	[SerializeField] private float _smoothTime = 0.15f;

	private Vector3 _velocity;
	private Quaternion _fixedRotation;

	private void Awake()
	{
		_fixedRotation = Quaternion.LookRotation((Vector3.up * 1.2f - _offset).normalized);
	}

	public void SetTarget(Transform target)
	{
		_target = target;
	}

	private void LateUpdate()
	{
		if (_target == null)
			return;

		Vector3 desired = _target.position + _offset;
		transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, _smoothTime);
		transform.rotation = _fixedRotation;
	}
}
