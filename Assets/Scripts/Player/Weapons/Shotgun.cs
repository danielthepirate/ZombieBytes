using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shotgun : Weapon {

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
	[SerializeField] LineRenderer[] traceLines;

	//LineRenderer traceLine;

	// Use this for initialization
	void Start () {
		//traceLine = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update() {
		ProcessWeaponInput();
	}

	public override void UseWeapon() {
		base.UseWeapon();

		muzzleFlash.Play();
		Invoke("Fire", fireDelay);
	}

	private void DisableTraceLine() {
		foreach(LineRenderer traceLine in traceLines) {
			traceLine.enabled = false;
		}
	}

	public void Fire() {
		DeductAmmo();

		foreach (LineRenderer traceLine in traceLines) {
			FirePellet(traceLine);
		}

		Invoke("DisableTraceLine", traceTime);
		AddCameraTrauma(trauma);
	}

	private void FirePellet(LineRenderer traceLine) {
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

		traceLine.SetPosition(0, muzzle.transform.position);
		traceLine.SetPosition(1, impactPoint);
		traceLine.enabled = true;
	}

	private void ApplyForceAlongRaycast( Ray weaponRay) {
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
		float offset = player.Accuracy() * spread;
		Vector3 originOffset;

		//direction
		Vector3 direction = player.TargetVector();
		direction = Quaternion.Euler(0f, Random.Range(-offset, offset), 0f) * direction;
		ray.direction = direction;

		//origin
		//origin is set offset relative to the direction to allow point blank shots
		originOffset = ray.origin - direction;
		originOffset = originOffset.normalized * 0.5f;
		ray.origin = muzzle.transform.position + originOffset;
	}
}
