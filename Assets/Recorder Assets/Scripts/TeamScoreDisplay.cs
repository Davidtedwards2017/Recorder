using UnityEngine;
using System.Collections;

public class TeamScoreDisplay : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture foregroundTexture;
	public Texture frameTexture;

	public int baseMarginLeft = 50;
	public int baseMarginTop = 50;

	public int baseWidth = 300;
	public int baseHeight = 300;

	public int progressWidth = 200;
	public int progressHeight = 200;

	public int progressMarginLeft = 10;
	public int progressMarginTop = 10;

	public int frameWidth = 200;
	public int frameHeight = 200;

	public int frameMarginLeft = 10;
	public int frameMarginTop = 10;

	void Start()
	{
		Vector3 screenVec = Camera.main.WorldToScreenPoint(transform.position);
		baseMarginLeft = (int)screenVec.x;
		baseMarginTop = Screen.height - (int)screenVec.y;
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(baseMarginLeft, baseMarginTop, baseWidth, baseHeight));
		GUI.DrawTexture(new Rect(frameMarginLeft, frameMarginTop, frameWidth, frameHeight), backgroundTexture, ScaleMode.ScaleToFit,true, 0);
		GUI.DrawTexture(new Rect(progressMarginLeft, progressMarginTop, progressWidth, progressHeight), foregroundTexture, ScaleMode.ScaleAndCrop,true, 0);
		GUI.DrawTexture(new Rect(frameMarginLeft, frameMarginTop, frameWidth, frameHeight), frameTexture, ScaleMode.ScaleToFit,true, 0);
		GUI.EndGroup();
	}

	public void UpdateScoreText(int score)
	{
	}
}
