using UnityEngine;
using System.Collections;

public class LeaderBoardScoreData  {

	public string leaderBoardId;

	public string leaderBoardScore;


	public float GetFloatScore() {
		return System.Convert.ToSingle (leaderBoardScore);
	}

	public float GetIntScore() {
		return System.Convert.ToInt64 (leaderBoardScore);
	}
}
