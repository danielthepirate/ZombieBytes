using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float healthCurrent = 4f;
	public float healthMaximum = 4f;

	public float stunDuration = 0.4f;

	[SerializeField] GameObject hitDecal;
	[SerializeField] GameObject hitFX;
	[SerializeField] GameObject ragdoll;

	GameObject bucketFX;
	GameObject bucketRagdolls;
	Rigidbody rigidBody;
	EnemyController zombie;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		bucketFX = GameObject.Find("BucketFX");
		bucketRagdolls = GameObject.Find("BucketRagdolls");
		zombie = GetComponent<EnemyController>();
	}

	public void Damage(float damageAmount, float knockBack, RaycastHit hitPoint) {
		CreateDamageFX(hitPoint);
		ApplyKnockback(knockBack, hitPoint, stunDuration);

		healthCurrent -= damageAmount;

		if (healthCurrent <= 0) {
			KillUnit();
		}
	}

	private void KillUnit() {
		zombie.state = EnemyController.State.Dead;

		Instantiate(ragdoll, transform.position, transform.rotation, bucketRagdolls.transform);

		//placeholder so there's always the same number of zombies
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		enemySpawner.SpawnZombie();

		Destroy(gameObject);
	}

	private void ApplyKnockback(float knockBack, RaycastHit hitPoint, float duration) {
		Vector3 force = -1 * hitPoint.normal * knockBack;
		rigidBody.AddForce(force, ForceMode.Impulse);
		zombie.state = EnemyController.State.Stun;
		zombie.ResetStateAfter(stunDuration);
	}

	private void CreateDamageFX(RaycastHit hitPoint) {
		var impactFX = Instantiate(hitFX, hitPoint.point, Quaternion.Euler(hitPoint.point), bucketFX.transform);
		impactFX.transform.forward = hitPoint.normal;

		SpawnDecalBloodPool();
	}

	private void SpawnDecalBloodPool() {
		float randomAngle = Random.Range(0f, 360f);
		float randomScale = Random.Range(-0.2f, 0.2f);

		GameObject decal = Instantiate(hitDecal, bucketFX.transform);
		decal.transform.position = transform.position;
		decal.transform.Rotate(0f, 0f, randomAngle);
		decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
	}
}
