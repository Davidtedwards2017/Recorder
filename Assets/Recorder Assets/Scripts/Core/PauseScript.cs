using UnityEngine;
using System.Collections;
using StageFramework;

public class PauseScript : MonoBehaviour {

	private Transform PauseMenu;

	public float DistanceFromCamera;

	// Use this for initialization
	void Start () {

		PauseMenu = transform.FindChild("PauseMenu");
		PauseMenu.gameObject.SetActive(false);

		CenterPause();

		GameStateManager.Instance.PausedEvent += onPause;
		GameStateManager.Instance.UnpausedEvent += onUnpause;
	}

	private void CenterPause()
	{
		float x = Screen.width / 2;
		float y = Screen.height /2;
		
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(x,y));
		
		transform.position = (ray.direction * DistanceFromCamera) + Camera.main.transform.position;
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
