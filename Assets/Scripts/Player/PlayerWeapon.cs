using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeapon : MonoBehaviour {

	[SerializeField] Text weaponText;
	public Weapon[] weapons;
	public Weapon activeWeapon;

	private void Start() {
		//Without this, AmmoCrate doesn't to be able to find the right Weapon unless it's been set to Active at least once
		Invoke("DefaultActiveWeapon", 0f);
	}

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

	private void DefaultActiveWeapon() {
		SetActiveWeapon(activeWeapon);
	}

	public void UpdateWeaponDisplay() {
		if(activeWeapon.UsesAmmo()) {
			weaponText.text = activeWeapon.WeaponName + " " + activeWeapon.ammoCount;
		}
		else {
			weaponText.text = activeWeapon.WeaponName;
		}

		if(activeWeapon.ammoCount != 0) {
			weaponText.color = Color.white;
		}
		else {
			weaponText.color = new Color(0.8f, 0.8f, 0.8f);
		}
	}
}
