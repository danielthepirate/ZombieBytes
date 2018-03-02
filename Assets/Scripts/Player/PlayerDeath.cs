using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

	public GameObject healthBar;
	public Vector3 damageOrigin;
	public float knockback;

	// Use this for initialization
	void Start () {
		AttachHealthBarToRagdoll();
		ApplyKnockbackToRagdoll();
	}

	private void AttachHealthBarToRagdoll() {
		GameObject body = transform.Find("Body").gameObject;
		AlignRectToObject healthBar = GameObject.Find("HealthBar").GetComponent<AlignRectToObject>();
		healthBar.followObject = body.gameObject;
	}

	private void ApplyKnockbackToRagdoll() {
		float forceFactor = 1f;
		Vector3 forceDirection = transform.position - damageOrigin;
		forceDirection = forceDirection.normalized;
		Vector3 force = forceDirection * knockback * forceFactor;

		Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in rbs) {
			rb.AddForce(force, ForceMode.Impulse);
		}
	}
}
