using UnityEngine;
using System.Collections;
using StageFramework;

public class PauseMenuScript : AMenuBase {

   	private PauseScript pauseMenu;

	// Use this for initialization
	protected override void Start () 
    {
        base.Start();
        pauseMenu = transform.parent.gameObject.GetComponent<PauseScript>();
	}

    protected override void Update()
    {
        base.Update();

        if(PlayerGamepadInput.GetStartButtonInput())
        {
            pauseMenu.UnpauseGame();
        }
    }

    protected override void SelectConfirmation()
    {
        //get GameMode
        string currentSelected =  SelectionGroups[CurrentSelectionGroupIndex].name;
        
        switch(currentSelected)
        {
            case "Resume":
                pauseMenu.UnpauseGame();
                break;
            case "Restart":
                GameInfo.Instance.CurrentGameMode.RestartGame();
                break;
            case "Quit":
                Time.timeScale = 1;
                Application.LoadLevel("StartScreen");
                break;
            default:
                break;
        } 
    }
    
    protected override void SelectCancel()
    {
        pauseMenu.UnpauseGame();
    }
}
