using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Unit {

	[SerializeField] GameObject decalPrefab;
	[SerializeField] GameObject hitFX;

	[SerializeField] GameObject ragdoll;

	float timer;
	PlayerController player;
	GameObject bucketRagdoll;
	Rigidbody rigidBody;
	Animator animator;

	float lookAngle = 50f;
	float lookSpeed = 1.4f;
	Quaternion defaulRotation;
	Quaternion targetRotation;

	bool isAlive;
	NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		defaulRotation = transform.rotation;
		rigidBody = GetComponent<Rigidbody>();
		nav = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		isAlive = true;
		player = FindObjectOfType<PlayerController>();
		bucketRagdoll = GameObject.Find("BucketRagdoll");
	}

	// Update is called once per frame
	void Update () {
		if (!isAlive) { return; }

		LookTowardsPlayer();
		MoveTowardsPlayer();
	}

	private void MoveTowardsPlayer() {
		if (player.IsAlive()) {
			nav.SetDestination(player.Position());
			animator.SetFloat("walkSpeed", 0.5f);
		}
		else {
			nav.enabled = false;
		}
	}

	private void LookTowardsPlayer() {
		Vector3 look = player.Position() - transform.position;
		look.y = 0f;

		Transform head = transform.Find("Head");

		Quaternion q = Quaternion.LookRotation(look);
		if (Quaternion.Angle(q, defaulRotation) <= lookAngle) {
			targetRotation = q;
		}

		if (Quaternion.Angle(transform.rotation, q) >= lookAngle) {
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotation, 0.01f);
		}
		else {
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
		}

	}

	public override void Damage(float damageAmount, float knockBack, RaycastHit hitPoint) {
		CreateDamageFX(hitPoint);
		ApplyKnockback(knockBack, hitPoint);

		healthCurrent -= damageAmount;

		if (healthCurrent <= 0) {
			KillUnit();
		}
	}

	private void KillUnit() {
		isAlive = false;

		Instantiate(ragdoll, transform.position, transform.rotation, bucketRagdoll.transform);

		//placeholder so there's always the same number of zombies
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		enemySpawner.SpawnZombie();

		Destroy(gameObject);
	}

	private void ApplyKnockback(float knockBack, RaycastHit hitPoint) {
		Vector3 force = -1 * hitPoint.normal * knockBack;
		rigidBody.AddForce(force, ForceMode.Impulse);
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
