﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Pistol : Weapon {

	[Header("Damage Data")]
	[SerializeField] float damage = 1f;
	[SerializeField] float range = 20f;
	[SerializeField] float spread = 0.5f;
	[SerializeField] float traceTime = 0.02f;
	[SerializeField] float knockbackForce = 1f;
	[SerializeField] float trauma = 0.1f;

	[Header("Launch Data")]
	[SerializeField] GameObject muzzle;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] float fireDelay = 0.1f;

	LineRenderer traceLine;

	// Use this for initialization
	void Start () {
		traceLine = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update() {
		traceLine.SetPosition(0, muzzle.transform.position);
		base.ProcessWeaponInput();
	}

	public override void UseWeapon() {
		base.UseWeapon();

		muzzleFlash.Play();
		Invoke("Fire", fireDelay);
	}

	private void DisableTraceLine() {
		traceLine.enabled = false;
	}

	public void Fire() {
		DeductAmmo();
		int targetable = LayerMask.GetMask("Enemy");

		Ray weaponRay = new Ray();
		RaycastHit hit;
		Vector3 impactPoint;

		ConstructWeaponRay(ref weaponRay);

		if (Physics.Raycast(weaponRay, out hit, range, targetable)) {
			impactPoint = hit.point;
			EnemyHealth unit = hit.collider.GetComponent<EnemyHealth>();

			if (unit) {
				unit.Damage(damage, knockbackForce, hit);
			}
		}
		else {
			impactPoint = weaponRay.origin + weaponRay.direction * range;
		}

		ApplyForceAlongRaycast(weaponRay);

		traceLine.SetPosition(1, impactPoint);
		traceLine.enabled = true;

		Invoke("DisableTraceLine", traceTime);
		AddCameraTrauma(trauma);
	}

	private void ApplyForceAlongRaycast(Ray weaponRay) {
		int ragdoll = LayerMask.GetMask("Ragdoll");

		Vector3 impactNormalized = weaponRay.direction.normalized * knockbackForce * 0.1f;
		RaycastHit[] physicsHits;
		physicsHits = Physics.RaycastAll(weaponRay, range, ragdoll);
		foreach (RaycastHit impact in physicsHits) {
			Rigidbody rb = impact.rigidbody;
			if (rb) {
				rb.AddForce(impactNormalized, ForceMode.Impulse);
			}
		}
	}

	private void ConstructWeaponRay(ref Ray ray) {
		float randomY = 0.5f;
		float offset = player.Accuracy() * spread;
		Vector3 originOffset;

		//direction
		Vector3 direction = player.TargetVector();
		direction.z += Random.Range(-offset, offset);
		direction.y += Random.Range(0, randomY);
		ray.direction = direction;

		//origin
		//origin is set offset relative to the direction to allow point blank shots
		originOffset = ray.origin - direction;
		originOffset = originOffset.normalized * 0.5f;
		ray.origin = muzzle.transform.position + originOffset;
	}
}
