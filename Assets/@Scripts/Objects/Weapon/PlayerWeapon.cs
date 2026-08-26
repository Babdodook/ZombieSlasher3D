using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
	[SerializeField] private string _projectilePrefabKey = "Projectile_Bullet";
	[SerializeField] private float _fireRate = 4f;
	[SerializeField] private float _damage = 5f;
	[SerializeField] private Transform _muzzle;

	private float _cooldown;

	public void SetFiring(bool held, Vector3 aimDir)
	{
		_cooldown -= Time.deltaTime;

		if (held && _cooldown <= 0f)
		{
			Fire(aimDir);
			_cooldown = 1f / _fireRate;
		}
	}

	private void Fire(Vector3 dir)
	{
		GameObject go = Managers.Resource.Instantiate(_projectilePrefabKey, pooling: true);
		if (go == null)
			return;

		Transform origin = _muzzle != null ? _muzzle : transform;
		go.transform.SetPositionAndRotation(origin.position, Quaternion.LookRotation(dir));

		Projectile projectile = go.GetComponent<Projectile>();
		projectile?.Launch(dir, _damage);
	}
}
