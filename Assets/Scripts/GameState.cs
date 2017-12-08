using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	static public bool gameOver;
	static public bool displayGameOver;
	static public int level;
	static public int score;
	static public int mistakes;
	static public int tutorialStep;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public static string GetScoreText() {
		return "SCORE:" + score.ToString().PadLeft(score.ToString().Length + 4, '0');
	}

	public static string GetMistakeText() {
		return "Mistakes X" + mistakes;
	}
	public static int GetScore() {
		return score;
	}

	public static int UpdateScore(int value) {
		score =+ value;
		return score;
	}

	public static int GetLevel() {
		return level;
	}

	public static int NextLevel() {
		level++;
		return level;
	}

}
