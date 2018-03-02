using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingScoreText : MonoBehaviour {

	[SerializeField] Transform canvas;
	[SerializeField] Text text;
	[SerializeField] float offset;

	public int score = 10;

	// Use this for initialization
	void Start () {
		//Transform canvas = GameObject.FindGameObjectWithTag("CanvasWorldUI").transform;
		gameObject.SetActive(true);
		//FormatScoreText();
	}

	private void OnValidate() {
		FormatScoreText();
	}

	public void SetWorldPosition(Transform spawnPosition) {
		Vector3 worldPosition = new Vector3(spawnPosition.position.x, spawnPosition.position.y + offset, spawnPosition.position.z);
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
		transform.position = screenPosition;
		FormatScoreText();
	}

	private void FormatScoreText() {
		text.text = "+" + score;
	}

	//public void Follow(GameObject followObject) {
	//	AlignWithObject alignWithObject = gameObject.GetComponent<AlignWithObject>();
	//	if (alignWithObject) {
	//		alignWithObject.followObject = followObject;
	//	}
	//}
}
