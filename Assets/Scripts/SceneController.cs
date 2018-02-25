using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour {

	[SerializeField] float reloadLevelDelay = 1f;
	int currentLevel;

	// Use this for initialization
	void Start () {
		currentLevel = SceneManager.GetActiveScene().buildIndex;
	}

	public void DelayedReloadLevel() {
		Invoke("ReloadLevel", reloadLevelDelay);
	}

	void ReloadLevel() {
		SceneManager.LoadScene(currentLevel);
	}
}
