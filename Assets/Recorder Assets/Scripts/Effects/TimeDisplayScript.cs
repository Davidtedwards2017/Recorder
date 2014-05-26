using UnityEngine;
using System.Collections;
using StageFramework;

public class TimeDisplayScript : MonoBehaviour {

	public float DistanceFromCamera;

	private int prevSeconds;
	private TextMesh textMesh;
	// Use this for initialization
	void Awake () {
        PositionDisplay();
		textMesh = GetComponentInChildren<TextMesh>();
	}

	void PositionDisplay()
	{
		float x = Screen.width / 2;
		float y = Screen.height / 2;
		
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(x,y));
		
		transform.position = (ray.direction * DistanceFromCamera) + Camera.main.transform.position;
	}
	
	public void UpdateTimeDisplay(float time)
	{
		int seconds = (int) time;

        if(prevSeconds == time)
        {
            return;
        }

        if(textMesh != null)
        {
            textMesh.text = string.Format("{0}:{1:00}",(seconds/60)%60,seconds%60);
        }

		//textMesh.text = value.ToString("F1");
        prevSeconds = seconds;
	}
}
