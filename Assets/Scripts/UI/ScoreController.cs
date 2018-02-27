using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	[Header("Components")]
	[SerializeField] Text[] digit;
	[SerializeField] ScoreMultiplier scoreMultiplier;

	[Header("Logic")]
	public long score;

	private void OnValidate() {
		FormatScore();
	}

	public void AddScore(int points) {
		score += (points * scoreMultiplier.multiplier);
		FormatScore();
	}

	private void FormatScore() {
		string displayScore;
		long minScore = 0;
		long maxScore = 999999999999;
		int displayLength = 12;

		//I feel like theres a better way to do pretty much all of the following:

		//ClampScore();
		if (score > maxScore) { score = maxScore; }
		if (score < minScore) { score = minScore; }
		displayScore = score.ToString();

		//PadScoreWithZeroes();
		while (displayScore.Length < displayLength) {
			displayScore = 0 + displayScore;
		}

		//ReverseScoreAsString();
		string displayScoreReverse = "";
		foreach (char c in displayScore) {
			displayScoreReverse = c + displayScoreReverse;
		}

		//AssignTextToDigitText();
		int m = 0;
		foreach (char c in displayScoreReverse){
			digit[m].text = c.ToString();
			m++;
		}
	}
}
