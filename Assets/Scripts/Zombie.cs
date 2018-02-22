using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit {

	[SerializeField] GameObject decalPrefab;
	[SerializeField] GameObject hitFX;

	float timer;
	GameObject player;

	float lookAngle = 50f;
	float lookSpeed = 1.4f;
	Quaternion defaulRotation;
	Quaternion targetRotation;

	// Use this for initialization
	void Start () {
		defaulRotation = transform.rotation;
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update () {
		LookTowardsPlayer();
	}

	private void LookTowardsPlayer() {
		Vector3 look = player.transform.position - transform.position;
		look.y = 0f;

		Quaternion q = Quaternion.LookRotation(look);
		if(Quaternion.Angle(q, defaulRotation) <= lookAngle) {
			targetRotation = q;
		}

		Transform head = transform.Find("Head");
		head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
	}

	public override void Damage(float damageAmount, float knockBack, RaycastHit hitPoint) {
		CreateDamageFX(hitPoint);
		ApplyKnockback(knockBack, hitPoint);

		healthCurrent -= damageAmount;

		if (healthCurrent <= 0) {
			Destroy(gameObject);
		}
	}

	private void ApplyKnockback(float knockBack, RaycastHit hitPoint) {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddExplosionForce(knockBack, hitPoint.point, 1f, 0f, ForceMode.Impulse);
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
