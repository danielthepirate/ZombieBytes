using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSetHeight : MonoBehaviour {

	public float height = 0.01f;

	// Use this for initialization
	void Start () {
		SetHeight();
	}

	private void OnValidate() {
		SetHeight();
	}

	private void SetHeight() {
		Vector3 pos = new Vector3(transform.position.x, height, transform.position.z);
		transform.position = pos;
	}
}
