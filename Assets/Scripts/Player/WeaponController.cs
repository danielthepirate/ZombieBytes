using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponController : MonoBehaviour {

	[Header("Weapon")]
	[SerializeField] float damage = 1f;
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
		int targetable = LayerMask.GetMask("Targetable");

		weaponTimer = cooldown;
		traceLine.enabled = true;
		traceLine.SetPosition(0, weapon.transform.position);

		Ray weaponRay = new Ray();
		RaycastHit hit;
		Vector3 impactPoint;

		ConstructWeaponRay(ref weaponRay);

		if (Physics.Raycast(weaponRay, out hit, range, targetable)) {
			impactPoint = hit.point;
			Unit unit = hit.collider.GetComponent<Unit>();

			if (unit) {
				unit.Damage(damage, knockbackForce, hit);
			}
		}
		else {
			impactPoint = weaponRay.origin + weaponRay.direction * range;
		}
		traceLine.SetPosition(1, impactPoint);
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
		ray.origin = weapon.transform.position + originOffset;
	}
}
