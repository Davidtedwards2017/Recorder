using UnityEngine;
using System.Collections;
using StageFramework;

public class PauseScript : MonoBehaviour {

	private Transform PauseMenu;

	// Use this for initialization
	void Start () {

		PauseMenu = transform.FindChild("PauseMenu");
		PauseMenu.gameObject.SetActive(false);

		GameStateManager.Instance.PausedEvent += onPause;
		GameStateManager.Instance.UnpausedEvent += onUnpause;
	}

	public void PauseGame()
	{
		GameStateManager.Instance.ReadyToPause();
	}
	
	public void UnpauseGame()
	{
		GameStateManager.Instance.ReadyToStartPlaying();
	}

	protected void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Start_Button"))
		{
			PauseGame();			
		}
	}

	void onPause(GameState prevState)
	{
        PauseMenu.gameObject.SetActive(true);
	}

	void onUnpause()
	{
        PauseMenu.gameObject.SetActive(false);
	}
}
