using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfAfter : MonoBehaviour {

	[SerializeField] float delay = 2f;

	void Start () {
		Destroy(gameObject, delay);
	}
}