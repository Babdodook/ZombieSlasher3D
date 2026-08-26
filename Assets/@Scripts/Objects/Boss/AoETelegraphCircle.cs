using System;
using UnityEngine;
using static Define;

public class AoETelegraphCircle : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	[SerializeField] private float _visualHeight = 0.05f;

	private float _radius;
	private float _duration;
	private float _damage;
	private float _timer;
	private Action _onResolved;
	private bool _resolved;

	public void Activate(Vector3 pos, float radius, float duration, float damage, Action onResolved)
	{
		transform.position = pos;
		_radius = radius;
		_duration = duration;
		_damage = damage;
		_timer = 0f;
		_onResolved = onResolved;
		_resolved = false;

		transform.localScale = Vector3.zero;
	}

	private void Update()
	{
		if (_resolved)
			return;

		_timer += Time.deltaTime;
		float t = Mathf.Clamp01(_timer / _duration);

		transform.localScale = new Vector3(_radius * 2f * t, _visualHeight, _radius * 2f * t);

		if (_renderer != null)
		{
			Color c = _renderer.material.color;
			c.a = Mathf.Lerp(0.15f, 0.6f, t);
			_renderer.material.color = c;
		}

		if (t >= 1f)
			Resolve();
	}

	private void Resolve()
	{
		_resolved = true;

		Collider[] hits = Physics.OverlapSphere(transform.position, _radius, 1 << (int)ELayer.Player);
		foreach (Collider col in hits)
		{
			CreatureObject target = col.GetComponentInParent<CreatureObject>();
			target?.TakeDamage(_damage);
		}

		_onResolved?.Invoke();
		Managers.Resource.Destroy(gameObject);
	}
}
