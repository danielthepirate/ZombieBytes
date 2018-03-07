using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

	public PlayerController player;

	[Header("Basic Properties")]
	public string WeaponName;
	public int ammoCount;
	public int ammoMax;

	public enum WeaponType { Pistol, SMG, Shotgun, AssaultRifle };
	public WeaponType weaponType;

	[SerializeField] float cooldown = 0.5f;

	PlayerWeapon weaponController;
	float weaponTimer;

	private void Awake() {
		weaponController = player.gameObject.GetComponent<PlayerWeapon>();
	}

	public virtual void ProcessWeaponInput() {
		weaponTimer -= Time.deltaTime;

		if (WeaponIsUseable()) {
			UseWeapon();
		}
	}

	public virtual bool WeaponIsUseable() {
		return CrossPlatformInputManager.GetButton("Fire") && OffCooldown() && HasAmmo();
	}

	private bool HasAmmo() {
		return !UsesAmmo() || ammoCount > 0;
	}

	private bool OffCooldown() {
		return weaponTimer < Mathf.Epsilon && Time.timeScale != 0;
	}

	public void Reload() {
		ammoCount = ammoMax;
		weaponController.UpdateWeaponDisplay();
	}

	public virtual void UseWeapon() {
		weaponTimer = cooldown;
	}

	public void DeductAmmo() {
		if (UsesAmmo()) {
			ammoCount -= 1;
			weaponController.UpdateWeaponDisplay();
		}
	}

	public bool UsesAmmo() {
		return ammoMax != -1;
	}

	public void AddCameraTrauma(float amount) {
		PlayerCamera playerCamera = player.GetComponent<PlayerHealth>().playerCamera;

		if (playerCamera) {
			playerCamera.AddTrauma(amount);
		}
	}
}
