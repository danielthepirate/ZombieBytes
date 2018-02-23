using System.Collections;
using UnityEngine;

public class FadeAndDestroyCoroutine : MonoBehaviour {

	[SerializeField] float fadeTime = 1f;
	[SerializeField] float delay = 2f;
	new Renderer renderer;
	TextMesh textMesh;

	void Start () {
		renderer = GetComponent<Renderer>();
		textMesh = GetComponent<TextMesh>();

		float destroyDelay = delay + 1f;
		Invoke("FadeAway", delay);
		Destroy(gameObject, destroyDelay);
	}

	private void FadeAway() {
		if (renderer) {
			StartCoroutine(MaterialFadeTo(0, fadeTime));
		}
		if (textMesh) {
			StartCoroutine(TextMeshFadeTo(0, fadeTime));
		}
	}

	IEnumerator MaterialFadeTo(float targetAlpha, float time) {
		float currentAlpha = renderer.material.color.a;

		float red = renderer.material.color.r;
		float green = renderer.material.color.g;
		float blue = renderer.material.color.b;
		float alpha;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time) {
			alpha = Mathf.Lerp(currentAlpha, targetAlpha, t);
			Color newColor = new Color(red, green, blue, alpha);
			renderer.material.color = newColor;
			yield return null;
		}
	}

	IEnumerator TextMeshFadeTo(float targetAlpha, float time) {
		float currentAlpha = textMesh.color.a;

		float red = textMesh.color.r;
		float green = textMesh.color.g;
		float blue = textMesh.color.b;
		float alpha;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time) {
			alpha = Mathf.Lerp(currentAlpha, targetAlpha, t);
			Color newColor = new Color(red, green, blue, alpha);
			textMesh.color = newColor;
			yield return null;
		}
	}
}