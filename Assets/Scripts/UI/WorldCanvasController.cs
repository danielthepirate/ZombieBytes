using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasController : MonoBehaviour {

	[SerializeField] GameObject floatingScoreText;

	public void CreateFloatingScoreText(Transform spawnPosition, int score) {
		GameObject newFloatingScoreText = Instantiate(floatingScoreText, transform);
		newFloatingScoreText.transform.SetParent(transform);

		FloatingScoreText control = newFloatingScoreText.GetComponent<FloatingScoreText>();

		control.score = score;
		control.SetWorldPosition(spawnPosition);
	}
}
