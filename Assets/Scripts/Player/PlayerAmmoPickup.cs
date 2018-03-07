using UnityEngine;

public class PlayerAmmoPickup : MonoBehaviour {

	[SerializeField] PlayerWeapon weaponController;

	private void OnTriggerEnter(Collider other) {
		AmmoCrate ammoCrate = other.gameObject.GetComponent<AmmoCrate>();

		if (ammoCrate) {
			ReplenishAmmo(ammoCrate);
		}
	}

	void ReplenishAmmo(AmmoCrate ammoCrate) {
		foreach(Weapon weapon in weaponController.weapons) {
			if(weapon.weaponType == ammoCrate.type) {
				weapon.ammoCount = weapon.ammoMax;
				Destroy(gameObject);
				return;
			}
		}
	}
}
