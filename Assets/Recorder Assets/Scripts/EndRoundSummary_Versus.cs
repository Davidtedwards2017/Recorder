using UnityEngine;
using System.Collections;
using StageFramework;

public class EndRoundSummary_Versus : MonoBehaviour {

    public Transform TextMeshPrefab;
    public float SpaceBetweenEntries;
    public GameObject[] EndScoreEntries;

    private int entryNumber = 0;

    // Use this for initialization
	void Start () {

        GameStateManager.Instance.RoundStartEvent += newRoundStarted;

        EndScoreEntries = new GameObject[2];
        TeamScore[] ts = GameInfo.Instance.CurrentGameMode.Score.TeamScores;

        //order scores from highest to lowerest
        //currently only mimic only supports 2 player


        if(ts[0].Score >= ts[1].Score)
        {
            CreateScoreEntry(0, ts[0].Score);
            CreateScoreEntry(1, ts[1].Score);
        }
        else if(ts[0].Score < ts[1].Score)
        {
            CreateScoreEntry(1, ts[1].Score);
            CreateScoreEntry(0, ts[0].Score);
        }
	}

    void CreateScoreEntry(int player, int value)
    {
        Vector3 entryPosition = transform.position;
        entryPosition.y -= SpaceBetweenEntries * entryNumber++;

        Transform T = Instantiate(TextMeshPrefab, entryPosition, Quaternion.identity) as Transform;
        T.parent = transform;
        T.name = "Player_" + (player+1) +" Score";
        TextMesh TM = T.gameObject.GetComponent<TextMesh>();

        TM.color = GameInfo.GetTeamColor(player + 1);
        TM.text = value.ToString();
    }
	

    void newRoundStarted()
    {
        GameStateManager.Instance.RoundStartEvent -= newRoundStarted;
        Destroy(gameObject);
    }
}
