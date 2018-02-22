using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit {

	[SerializeField] GameObject decalPrefab;
	[SerializeField] GameObject hitFX;

	float timer;

	// Use this for initialization
	void Start () {

	}

	private void SpawnDecalBloodPool() {
		float randomAngle = Random.Range(0f, 360f);
		float randomScale = Random.Range(-0.2f, 0.2f);

		GameObject decal = Instantiate(decalPrefab);
		decal.transform.position = transform.position;
		decal.transform.Rotate(0f, 0f, randomAngle);
		decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
	}

	// Update is called once per frame
	void Update () {

	}

	public override void Damage(int amount, float knockBack, RaycastHit hitPoint) {
		SpawnDecalBloodPool();

		if (isDead) { return; }

		var impactFX = Instantiate(hitFX, hitPoint.point, Quaternion.Euler(hitPoint.point));
		impactFX.transform.forward = hitPoint.normal;
		
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddExplosionForce(knockBack, hitPoint.point, 1f, 0f, ForceMode.Impulse);
	}
}
