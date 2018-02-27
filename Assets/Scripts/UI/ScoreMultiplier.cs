using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScoreMultiplier : MonoBehaviour {

	[Header("Components")]
	[SerializeField] Image image;
	[SerializeField] Image endCap;
	[SerializeField] Text text;

	[Header("Logic")]
	[Range(1,999)]public int multiplier = 1;
	public float baseMultiplierTime = 3.0f;
	public float multiplierTime;
	public float timer;

	float multiplierScaling = 20f;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		ProcessMultiplierTimer();
		image.fillAmount = timer / multiplierTime;
		image.color = new Color(1f, 1f, 1f, image.fillAmount + 0.05f);

		RotateEndCap();
	}

	private void ProcessMultiplierTimer() {
		if(multiplier == 1) { return; }

		timer -= Time.deltaTime;
		if (timer < Mathf.Epsilon) {
			SetMultiplier(multiplier - 1);
			timer = multiplierTime;
		}
	}

	private void RotateEndCap() {
		float fill = image.fillAmount;
		float angle = 360 - (360 * fill);
		Vector3 newRotation = new Vector3(35f, 0f, angle);
		endCap.transform.rotation = Quaternion.Euler(newRotation);
	}

	private void OnValidate() {
		SetMultiplier(multiplier);
		if (timer < Mathf.Epsilon) {
			timer = multiplierTime;
		}
	}

	public void SetMultiplier(int m){
		multiplier = m;
		text.text = "x" + multiplier;
		multiplierTime = baseMultiplierTime - Mathf.Log(multiplier, multiplierScaling);
		
		//todo some modification if multiplier == 1
	}
}
