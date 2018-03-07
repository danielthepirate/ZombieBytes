using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeapon : MonoBehaviour {

	[SerializeField] Text weaponText;
	[SerializeField] Weapon[] weapons;
	public Weapon activeWeapon;

	private void Update() {
		float scroll = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
		
		if (scroll > 0) {
			NextActiveWeapon(1);
		}
		else if (scroll < 0) {
			NextActiveWeapon(-1);
		}
	}

	private void NextActiveWeapon(int index) {
		int activeIndex = Array.IndexOf(weapons, activeWeapon);
		int nextIndex = (activeIndex + index + weapons.Length) % weapons.Length;
		SetActiveWeapon(weapons[nextIndex]);
	}

	public void SetActiveWeapon(Weapon newActiveWeapon) {
		foreach (Weapon weapon in weapons) {
			weapon.gameObject.SetActive(false);
		}
		activeWeapon = newActiveWeapon;
		activeWeapon.gameObject.SetActive(true);
		UpdateWeaponDisplay();
	}

	public void UpdateWeaponDisplay() {
		if(activeWeapon.UsesAmmo()) {
			weaponText.text = activeWeapon.WeaponName + " " + activeWeapon.ammoCount;
		}
		else {
			weaponText.text = activeWeapon.WeaponName;
		}
	}
}
