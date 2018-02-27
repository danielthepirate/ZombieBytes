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
	public bool isEnabled = true;
	[Range(1,999)] public int multiplier = 1;
	public float baseMultiplierTime = 3.0f;
	public float multiplierTime;

	float timer;
	float multiplierScaling = 50f;

	//Update on editor value change
	private void OnValidate() {
		Set(multiplier);
		if (timer < Mathf.Epsilon) {
			ResetTimer();
		}
	}

	// Update is called once per frame
	void Update() {
		if (!MultiplierIsRunning()) { return; }
		ProcessMultiplierTimer();
		ApplyMultiplierFill();
		ApplyEndCapRotation();
	}

	public bool MultiplierIsRunning() {
		return isEnabled && multiplier > 1;
	}

	private void ApplyMultiplierFill() {
		Color fillColor;
		image.fillAmount = timer / multiplierTime;
		if (multiplier > 1) {
			fillColor = new Color(1f, 1f, 1f, image.fillAmount * 0.5f);
		}
		else {
			fillColor = new Color(1f, 1f, 1f, 0f);
		}
		image.color = fillColor;
	}

	private void ProcessMultiplierTimer() {
		if(multiplier == 1) { return; }

		timer -= Time.deltaTime;
		if (timer < Mathf.Epsilon) {
			Set(multiplier - 1);
			timer = multiplierTime;
		}
	}

	private void ApplyEndCapRotation() {
		float fill = image.fillAmount;
		float angle = 360 - (360 * fill);
		Vector3 newRotation = new Vector3(35f, 0f, angle);
		endCap.transform.rotation = Quaternion.Euler(newRotation);
	}


	public void Set(int m) {
		multiplier = m;
		UpdateMultiplier();
	}

	public void Increment() {
		multiplier += 1;
		UpdateMultiplier();
		ResetTimer();
	}

	private void ResetTimer() {
		timer = multiplierTime;
	}

	public void Decrement() {
		multiplier -= 1;
		UpdateMultiplier();
	}

	private void UpdateMultiplier() {
		multiplier = Mathf.Clamp(multiplier, 1, 999);
		text.text = "x" + multiplier;
		multiplierTime = baseMultiplierTime - Mathf.Log(multiplier, multiplierScaling);
	}
}
