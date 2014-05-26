using UnityEngine;
using System.Collections;
using StageFramework;

public class TimeScaleIndpendentUpdate : MonoBehaviour {

	public bool pausedWhenGameIsPaused = true;

	float previousTimeSinceStartup;

	protected virtual void Awake()
	{
		previousTimeSinceStartup = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	protected virtual void Update () 
	{
		float realTimeSinceStartup = Time.realtimeSinceStartup;
		deltaTime = realTimeSinceStartup - previousTimeSinceStartup;
		previousTimeSinceStartup = realTimeSinceStartup;

		if(deltaTime < 0)
		{
			deltaTime = 0;
		}

		if(pausedWhenGameIsPaused && IsGamePaused())
		{
			deltaTime = 0;
		}
	}

	public IEnumerator TimeScaleIndependentWaitForSeconds(float seconds)
	{
		float elapsedTime = 0;
		while(elapsedTime < seconds)
		{
			yield return null;
			elapsedTime += deltaTime;
		}
	}

	private bool IsGamePaused()
	{
		if(GameStateManager.Instance != null)
			return GameStateManager.Instance.CurrentGameState == GameState.Paused;

		return false;
	}

    public float deltaTime { get; private set;}
}
