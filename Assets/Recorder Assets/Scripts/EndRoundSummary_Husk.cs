using UnityEngine;
using System.Collections;
using StageFramework;

public class EndRoundSummary_Husk : MonoBehaviour {

    TextMesh scoreValueTextMesh;
	// Use this for initialization
	void Start () 
    {
        GameStateManager.Instance.RoundStartEvent += newRoundStarted;
        scoreValueTextMesh = transform.FindChild("Score Value Text").GetComponent<TextMesh>();

        int endScore = GameInfo.Instance.CurrentGameMode.Score.TeamScores[0].Score;
        scoreValueTextMesh.text = endScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void newRoundStarted()
    {
        GameStateManager.Instance.RoundStartEvent -= newRoundStarted;
        Destroy(gameObject);
    }


    //void Get
}
