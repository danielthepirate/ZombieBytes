using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class ComboController : MonoBehaviour {

	//[SerializeField] GameObject scoreDisplayText;

	//Text scoreDisplay;
	//int score = 0;
	//float updateTime = 0f;
	//float updateTick = 0.5f;

	//bool isAnimating = false;
	//Animator animator;

	// Use this for initialization
	void Start() {
		//scoreDisplay = scoreDisplayText.GetComponent<Text>();
		//animator = scoreDisplayText.GetComponent<Animator>();

		//UpdateScoreDisplay();
	}

	//private void UpdateScoreDisplay() {
	//	scoreDisplay.text = "x" + score;

	//	//this worked before but then you broke something in the animator window
	//	//figure that out and then come back to this
	//	animator.SetBool("EventUpdate", true);
	//	Invoke("ResetAnimator", 0.5f);
	//}

	//private void ResetAnimator(){
	//	animator.SetBool("EventUpdate", false);
	//}

	// Update is called once per frame
	void Update () {
		//if(Time.time > updateTime) {
		//	updateTime += updateTick;
		//	score += 1;
		//	UpdateScoreDisplay();
		//}
	}
}
