using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAndDestroyAfter : MonoBehaviour {

	[SerializeField] float delay = 2f;

	void Start () {
		float destroyDelay = delay + 1f;
		Invoke("FadeAway", delay);
		Destroy(gameObject, destroyDelay);
	}

	private void FadeAway() {
		Animator anim = GetComponent<Animator>();
		anim.SetTrigger("FadeOut");
	}
}