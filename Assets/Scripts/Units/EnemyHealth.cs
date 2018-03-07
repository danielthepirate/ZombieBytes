using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	[Header("Health Data")]
	public float healthCurrent = 4f;
	public float healthMaximum = 4f;

	public float stunFactor = 0.04f;

	[Header("Components")]
	[SerializeField] GameObject hitDecal;
	[SerializeField] GameObject hitFX;
	[SerializeField] GameObject ragdoll;
	[SerializeField] GameObject floatingScoreText;

	[Header("Loot Table")]
	[SerializeField] Loot loot;

	GameObject bucketFX;
	Rigidbody rb;
	EnemyController zombie;
	int score;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		zombie = GetComponent<EnemyController>();

		bucketFX = GameObject.Find("BucketFX");
	}

	public void Damage(float damageAmount, float knockBack, RaycastHit hitPoint) {
		CreateDamageFX(hitPoint);
		ApplyKnockback(knockBack, hitPoint);

		healthCurrent -= damageAmount;

		if (healthCurrent <= 0) {
			KillUnit();
		}
	}

	private void KillUnit() {
		zombie.state = EnemyController.State.Dead;
		Instantiate(ragdoll, transform.position, transform.rotation);

		AddScore();
		SpawnScoreText();
		SpawnLoot();

		//placeholder so there's always the same number of zombies
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		enemySpawner.SpawnZombie();

		Destroy(gameObject);
	}

	private void SpawnLoot() {
		int roll = Random.Range(1, 100);

		if(roll <= loot.dropChance) {
			Vector3 lootPosition = new Vector3(transform.position.x, 0.8f, transform.position.z);
			GameObject newLoot = Instantiate(loot.item);
			newLoot.transform.position = lootPosition;
		}


	}

	private void SpawnScoreText() {
		GameObject newFloatingScoreText = Instantiate(floatingScoreText);
		FloatingScoreText controller = newFloatingScoreText.GetComponent<FloatingScoreText>();
		controller.SetWorldPosition(zombie.transform);
		controller.SetScore(score);
	}

	private void AddScore() {
		ScoreController scoreController = GameObject.FindGameObjectWithTag("ScoreController").GetComponent<ScoreController>();
		ScoreMultiplier scoreMultiplier = GameObject.FindGameObjectWithTag("ScoreMultiplier").GetComponent<ScoreMultiplier>();
		score = zombie.score * scoreMultiplier.multiplier;
		scoreController.AddScore(score);
		scoreMultiplier.Increment();
	}

	private void ApplyKnockback(float knockBack, RaycastHit hitPoint) {
		float stunDuration = knockBack * stunFactor;
		Vector3 force = -1 * hitPoint.normal * knockBack;
		rb.AddForce(force, ForceMode.Impulse);
		zombie.state = EnemyController.State.Stun;
		zombie.ResetStateAfter(stunDuration);
	}

	private void CreateDamageFX(RaycastHit hitPoint) {
		var impactFX = Instantiate(hitFX, hitPoint.point, Quaternion.Euler(hitPoint.point), bucketFX.transform);
		impactFX.transform.forward = hitPoint.normal;

		CreateHitDecal();
	}

	private void CreateHitDecal() {
		float randomAngle = Random.Range(0f, 360f);
		float randomScale = Random.Range(-0.2f, 0.2f);

		GameObject decal = Instantiate(hitDecal, bucketFX.transform);
		decal.transform.position = transform.position;
		decal.transform.Rotate(0f, 0f, randomAngle);
		decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
	}
}
