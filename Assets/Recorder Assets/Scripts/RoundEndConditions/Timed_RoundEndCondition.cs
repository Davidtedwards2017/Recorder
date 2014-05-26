using UnityEngine;
using System.Collections;
using StageFramework;

public class Timed_RoundEndCondition : aRoundEndCondition {

	public float MaxTime;
	public float CurrentTime;
	private bool m_Started = false;

	public TimeDisplayScript TimeDisplay;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		TimeDisplay = GameObject.Find("TimeDisplay").GetComponent<TimeDisplayScript>();
	}

	protected override void onRoundStart()
	{
		CurrentTime = MaxTime;
	}

	protected override void onStartedPlaying(GameState prevState)
	{
		m_Started = true;
	}

	protected override void onRoundEnded()
	{
		m_Started = false;
	}

	void Update()
	{
		
		if(m_Started)
		{
			CurrentTime -= Time.deltaTime;

			if(CurrentTime <= 0)
			{
				CurrentTime = 0;
				onRoundEndConditionMet();
			}
		}

		TimeDisplay.UpdateTimeDisplay(CurrentTime);

	}



}
