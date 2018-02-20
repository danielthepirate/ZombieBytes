using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponController : MonoBehaviour {

	[Header("Weapon")]
	[SerializeField] float cooldown = 0.5f;
	[SerializeField] float range = 20f;
	[SerializeField] float spread = 0.5f;
	[SerializeField] float traceTime = 0.02f;
	[SerializeField] float knockbackForce = 1f;

	PlayerController player;
	Weapon weapon;
	LineRenderer traceLine;
	float weaponTimer;

	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerController>();
		weapon = GetComponentInChildren<Weapon>();
		traceLine = weapon.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update() {
		weaponTimer -= Time.deltaTime;

		if (CrossPlatformInputManager.GetButton("Fire") && weaponTimer < Mathf.Epsilon && Time.timeScale != 0) {
			Fire();
		}
		if (weaponTimer < cooldown - traceTime) {
			traceLine.enabled = false;
		}
	}

	public void Fire() {
		int shootable = LayerMask.GetMask("Shootable");

		weaponTimer = cooldown;
		traceLine.enabled = true;
		traceLine.SetPosition(0, weapon.transform.position);

		Vector3 impactPoint;
		Ray bullet = new Ray();
		RaycastHit hit;
		bullet.origin = weapon.transform.position;
		bullet.direction = BulletDirection();

		if (Physics.Raycast(bullet, out hit, range, shootable)) {
			impactPoint = hit.point;
			Unit unit = hit.collider.GetComponent<Unit>();

			if (unit) {
				unit.Damage(0, knockbackForce, hit);
			}
		}
		else {
			impactPoint = bullet.origin + bullet.direction * range;
		}
		traceLine.SetPosition(1, impactPoint);
	}

	private Vector3 BulletDirection() {
		float randomY = 0.5f;
		float offset = player.Accuracy() * spread;

		Vector3 direction = player.TargetVector();
		direction.z += Random.Range(-offset, offset);
		direction.y += Random.Range(0, randomY);

		return direction;
	}
}
