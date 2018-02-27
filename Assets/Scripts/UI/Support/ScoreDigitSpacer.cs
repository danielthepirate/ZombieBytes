using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScoreDigitSpacer : MonoBehaviour {

	[SerializeField] float spacing = 58f;
	[SerializeField] float width = 60f;
	[SerializeField] float offset = -27.5f;

	[SerializeField] Text[] digits;

	private void Update() {

		float x = offset;

		foreach (Text digit in digits) {
			digit.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, width);
			x -= spacing;
		}
	}
}
