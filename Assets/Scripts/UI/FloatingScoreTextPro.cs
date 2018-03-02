using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingScoreTextPro : MonoBehaviour {

	[SerializeField] float offset;
	public int debugScore;

	TextMeshPro textMesh;

	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMeshPro>();
		gameObject.SetActive(true);

		SetScore(99);
	}

	private void OnValidate() {
		SetScore(debugScore);
	}

	public void SetWorldPosition(Transform spawn) {
		Vector3 worldPosition = new Vector3(spawn.position.x, spawn.position.y, spawn.position.z);
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
		transform.position = screenPosition;
		print(spawn.position);
	}

	public void SetScore(int score) {
		textMesh = GetComponent<TextMeshPro>();
		textMesh.SetText("+" + score);
	}
}
