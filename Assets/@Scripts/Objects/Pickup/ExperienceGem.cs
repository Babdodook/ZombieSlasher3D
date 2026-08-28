using UnityEngine;
using static Define;

public class ExperienceGem : BaseObject
{
	[SerializeField] private float _pullSpeed = 12f;

	private float _xpValue;
	private bool _pulled;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Pickup;
		return true;
	}

	// Pooled re-init entry point, same role as Projectile.Launch.
	public void Activate(Vector3 pos, float xpValue)
	{
		transform.position = pos;
		_xpValue = xpValue;
		_pulled = false;
	}

	private void Update()
	{
		Transform player = Managers.Level.Player;
		PlayerRunStats stats = Managers.Level.Stats;
		if (player == null || stats == null)
			return;

		Vector3 toPlayer = player.position - transform.position;

		if (_pulled == false && toPlayer.magnitude <= stats.PickupRadius)
			_pulled = true;

		if (_pulled)
			transform.position += toPlayer.normalized * _pullSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != (int)ELayer.Player)
			return;

		Managers.Level.AddXp(_xpValue);
		Managers.Resource.Destroy(gameObject);
	}
}
