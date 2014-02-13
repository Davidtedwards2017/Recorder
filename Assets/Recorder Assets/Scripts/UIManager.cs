using UnityEngine;
using System.Collections;
using StageFramework;

public class UIManager : MonoBehaviour {

	private GameInfo GameInfo;
	void Awake()
	{
		this.GameInfo = GetComponent<GameInfo>();
	}

	// Update is called once per frame
	void OnGUI () 
	{
		switch (this.GameInfo.GSM.CurrentGameState)
		{
		case GameState.Loading:
			UpdateLoading();
			break;
		case GameState.Loaded:
			break;
		case GameState.GameStart:
			UpdateGameStart();
			break;
		case GameState.RoundStart:
			UpdateRoundStart();
			break;
		case GameState.Paused:
			UpdatePaused();
			break;
		case GameState.Playing:
			UpdatePlaying();
			break;
		case GameState.RoundEnd:
			UpdateRoundEnd();
			break;
		case GameState.GameEnd:
			UpdateGameEnd();
			break;
		default:
			break;
		}
	}

	void UpdateLoading()
	{
	}

	void UpdateGameStart()
	{
	}

	void UpdateRoundStart()
	{
	}

	void UpdatePaused()
	{
	}

	void UpdatePlaying()
	{
	}

	void UpdateRoundEnd()
	{
	}

	void UpdateGameEnd()
	{
	}

}
