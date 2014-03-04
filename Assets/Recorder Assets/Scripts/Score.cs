using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	private TeamScore[] TeamScores;

	public int MaxScore;
	public Transform ScoreContainerPrefab;

	public Transform[] ScoreDisplayAnchors;
	
	// Use this for initialization
	void Start () {
		InitializeScore(1);
	}

	public void InitializeScore(int numOfTeams)
	{
		TeamScores = new TeamScore[numOfTeams];


		for( int i = 0; i < TeamScores.Length; i++)
		{
			if(ScoreDisplayAnchors[i] == null)
			{
				Debug.LogWarning("[Score.InitializeScore] ScoreDisplayAnchors["+i+"] not set");
				continue;
			}

			Transform t = Instantiate(ScoreContainerPrefab, ScoreDisplayAnchors[i].transform.position, Quaternion.identity) as Transform;
			TeamScores[i] = new TeamScore(i+1, t.gameObject);

		}
	}

	public void ResetScore()
	{
	}

	public void AddToScore(int team, int amt)
	{
		if(TeamScores[team-1] == null)
			return;

		TeamScores[team-1].AddToScore(amt);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}

class TeamScore
{
	public int Score;
	public GameObject TeamScoreContainer;

	public TeamScore(int Team, GameObject Container)
	{
		Container.name = "TeamScore_" + Team;
	}

	public void AddToScore(int amt)
	{

		Score += amt;
		TeamScoreContainer.GetComponent<TeamScoreDisplay>();
	}
}


