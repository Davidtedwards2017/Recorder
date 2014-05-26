using UnityEngine;
using System.Collections;

public class TeamScoreDisplay : MonoBehaviour {

	TextMesh textMesh;

    void Start()
	{
        textMesh = GetComponentInChildren<TextMesh>();
	}

    public void SetScoreDisplayText(int score, int multiplier)
	{
        textMesh.text = string.Format("{0}X {1}", multiplier, score );
	}
}
