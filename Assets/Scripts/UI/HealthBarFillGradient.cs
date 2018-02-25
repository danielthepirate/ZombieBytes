using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFillGradient : MonoBehaviour {

	[SerializeField] Gradient gradient;

	Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		float key = image.fillAmount;
		image.color = gradient.Evaluate(key);

	}
}
