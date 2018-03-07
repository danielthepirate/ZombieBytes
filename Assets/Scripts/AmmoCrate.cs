using UnityEngine;

public class AmmoCrate : MonoBehaviour {
	public Weapon.WeaponType type;

	PlayerWeapon weaponController;

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
				Destroy(gameObject);
				return;
			}
		}
	}
}
