using UnityEngine;
using static Define;

public class Projectile : BaseObject
{
	[SerializeField] private float _speed = 20f;
	[SerializeField] private float _lifetime = 3f;

	private Vector3 _dir;
	private float _damage;
	private float _timer;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Projectile;
		return true;
	}

	public void Launch(Vector3 dir, float damage)
	{
		_dir = dir.normalized;
		_damage = damage;
		_timer = 0f;
	}

	private void Update()
	{
		transform.position += _dir * _speed * Time.deltaTime;

		_timer += Time.deltaTime;
		if (_timer > _lifetime)
			Managers.Resource.Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != (int)ELayer.Monster)
			return;

		CreatureObject target = other.GetComponentInParent<CreatureObject>();
		target?.TakeDamage(_damage);

		Managers.Resource.Destroy(gameObject);
	}
}
