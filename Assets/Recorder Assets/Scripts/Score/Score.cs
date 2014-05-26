using UnityEngine;
using System.Collections;
using StageFramework;

public class Score : MonoBehaviour {

	public TeamScore[] TeamScores;

    public float scoreDisplayPositionOffset;

	public int MaxScore;
	public Transform ScoreContainerPrefab;

	public void InitializeScore(int numOfTeams)
	{
		TeamScores = new TeamScore[numOfTeams];

        Transform scoreDisplayAnchor = GameObject.Find("TimeDisplay").transform;

		for( int i = 0; i < TeamScores.Length; i++)
		{
		
            Vector3 position = scoreDisplayAnchor.position;

            switch (i)
            {
                case 0:
                    position.y += scoreDisplayPositionOffset;
                    break;
                case 1:
                    position.y -= scoreDisplayPositionOffset;
                    break;
            }

            Transform t = Instantiate(ScoreContainerPrefab, position, Quaternion.identity) as Transform;
			TeamScores[i] = t.GetComponent<TeamScore>();

			//set for team
			TeamScores[i].name = "TeamScore_" + i+1;
            TeamScores[i].ScoreChangedEvent += onScoreChanged;

            //set team text color
            t.gameObject.GetComponentInChildren<TextMesh>().color = GameInfo.GetTeamColor(i+1);
		}
	}


    public void ResetScores()
	{
		foreach (TeamScore score in TeamScores)
			score.ResetScore();
	}


	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddToTeamScore(int team, int amount)
    {
        if(IsValidTeam(team))
        {
            TeamScores[team-1].Add(amount);
        }
    }

    public void ResetTeamScoreMultiplier(int team)
    {
        if(IsValidTeam(team))
        {
            TeamScores[team-1].ScoreMultiplier = 1;
        }
    }

	void onScoreChanged(TeamScore teamScoreScript, int newScore)
	{

	}

    bool IsValidTeam(int team)
    {
        if(team > GameInfo.Instance.CurrentGameMode.NumberOfPlayers)
        {
            return false;
        }

        return true;
    }
	
}


