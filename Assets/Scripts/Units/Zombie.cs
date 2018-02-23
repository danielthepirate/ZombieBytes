using UnityEngine;
using UnityEngine.AI;

public class Zombie : Unit {

	[SerializeField] GameObject decalPrefab;
	[SerializeField] GameObject hitFX;

	[SerializeField] GameObject ragdoll;

	public float stunDuration = 0.4f;

	float timer;
	PlayerController player;
	GameObject bucketRagdoll;
	Rigidbody rigidBody;
	Animator animator;

	float lookAngle = 50f;
	float lookSpeed = 1.4f;
	Quaternion targetRotation;

	NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		nav = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		player = FindObjectOfType<PlayerController>();
		bucketRagdoll = GameObject.Find("BucketRagdoll");
	}

	// Update is called once per frame
	void Update () {
		if (isDead) { return; }

		LookTowardsPlayer();
		MoveTowardsPlayer();
	}

	private void MoveTowardsPlayer() {
		if (!player.isDead && !isStun) {
			nav.enabled = true;
			nav.SetDestination(player.Position());
			animator.SetFloat("walkSpeed", 0.5f);
		}
		else {
			nav.enabled = false;
			animator.SetFloat("walkSpeed", 0f);
		}
	}

	private void LookTowardsPlayer() {
		Vector3 look = player.Position() - transform.position;
		look.y = 0f;

		Transform head = transform.Find("Head");

		Quaternion rotationToTarget = Quaternion.LookRotation(look);
		if (Quaternion.Angle(rotationToTarget, transform.localRotation) <= lookAngle) {
			targetRotation = rotationToTarget;
		}

		if (Quaternion.Angle(transform.rotation, rotationToTarget) > lookAngle) {
			//this keeps head angle within natural limits
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, transform.localRotation, Time.deltaTime * lookSpeed);
		}
		else {
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
		}

	}

	public override void Damage(float damageAmount, float knockBack, RaycastHit hitPoint) {
		CreateDamageFX(hitPoint);
		ApplyKnockback(knockBack, hitPoint, stunDuration);

		healthCurrent -= damageAmount;

		if (healthCurrent <= 0) {
			KillUnit();
		}
	}

	private void KillUnit() {
		isDead = true;

		Instantiate(ragdoll, transform.position, transform.rotation, bucketRagdoll.transform);

		//placeholder so there's always the same number of zombies
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		enemySpawner.SpawnZombie();

		Destroy(gameObject);
	}

	private void ApplyKnockback(float knockBack, RaycastHit hitPoint, float duration) {
		Vector3 force = -1 * hitPoint.normal * knockBack;
		rigidBody.AddForce(force, ForceMode.Impulse);
		isStun = true;
		Invoke("Unstun", duration);
	}

	private void Unstun() {
		isStun = false;
	}

	private void CreateDamageFX(RaycastHit hitPoint) {
		var impactFX = Instantiate(hitFX, hitPoint.point, Quaternion.Euler(hitPoint.point));
		impactFX.transform.forward = hitPoint.normal;

		SpawnDecalBloodPool();
	}

	private void SpawnDecalBloodPool() {
		float randomAngle = UnityEngine.Random.Range(0f, 360f);
		float randomScale = UnityEngine.Random.Range(-0.2f, 0.2f);

		GameObject decal = Instantiate(decalPrefab);
		decal.transform.position = transform.position;
		decal.transform.Rotate(0f, 0f, randomAngle);
		decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
	}
}
