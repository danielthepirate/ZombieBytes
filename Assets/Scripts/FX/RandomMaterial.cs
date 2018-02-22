using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class RandomMaterial : MonoBehaviour {

	[SerializeField] Material[] materialPool;

	// Use this for initialization
	void Start () {
		int randomIndex = Random.Range(0, materialPool.Length);
		Renderer renderer = gameObject.GetComponent<Renderer>();
		renderer.material = materialPool[randomIndex];
	}
}
