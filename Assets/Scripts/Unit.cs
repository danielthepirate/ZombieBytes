using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public float healthMax;
	public float healthCurrent;

	public bool isDead;

	public virtual void Damage (int amount, float knockback, RaycastHit hitPoint) {

	}
}
