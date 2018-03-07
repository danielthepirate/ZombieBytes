using UnityEngine;

[ExecuteInEditMode]
public class EquipWeaponsInEditMode : MonoBehaviour {

	[SerializeField] PlayerWeapon playerWeapon;

	// Update is called once per frame
	void Update () {
		playerWeapon.SetActiveWeapon(playerWeapon.activeWeapon);
	}
}
