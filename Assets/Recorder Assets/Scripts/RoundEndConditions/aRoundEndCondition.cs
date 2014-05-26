using UnityEngine;
using System.Collections;
using StageFramework;

public abstract class aRoundEndCondition : MonoBehaviour {
	
	#region Event declaration
	public delegate void RoundEndConditionMetEventHandler(aRoundEndCondition condition);
	public event RoundEndConditionMetEventHandler RoundEndConditionMetEvent;
		
	protected virtual void onRoundEndConditionMet() 
	{ 
		if(RoundEndConditionMetEvent != null) 
			RoundEndConditionMetEvent(this); 
	}

	#endregion

	protected virtual void Start()
	{
		GameStateManager.Instance.RoundStartEvent += onRoundStart;
		GameStateManager.Instance.StartPlayingEvent += onStartedPlaying;
		GameStateManager.Instance.RoundEndEvent += onRoundEnded;
	}

	protected abstract void onRoundStart();
	protected abstract void onStartedPlaying(GameState prevState);
	protected abstract void onRoundEnded();	
}
