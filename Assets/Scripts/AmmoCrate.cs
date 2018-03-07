using System;
using UnityEngine;

public class AmmoCrate : MonoBehaviour {

	[SerializeField] Collider collidable;

	public Weapon.WeaponType type;

	GameObject bucketPickups;
	PlayerWeapon weaponController;
	Rigidbody rb;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		bucketPickups = GameObject.Find("BucketPickups");
		if (bucketPickups) {
			transform.parent = bucketPickups.transform;
		}
	}

	private void OnTriggerEnter(Collider other) {
		weaponController = other.gameObject.GetComponent<PlayerWeapon>();

		if (weaponController) {
			ReplenishAmmo();
		}
	}

	void ReplenishAmmo() {
		foreach (Weapon weapon in weaponController.weapons) {
			if (weapon.weaponType == type) {
				weapon.Reload();
				StartDestroySequence();
				return;
			}
		}
	}

	private void StartDestroySequence() {
		rb.constraints = RigidbodyConstraints.FreezePosition;
		Destroy(collidable);
		FadeAndDestroyChildrenCoroutine destroySequence = GetComponent<FadeAndDestroyChildrenCoroutine>();
		destroySequence.enabled = true;
	}
}
