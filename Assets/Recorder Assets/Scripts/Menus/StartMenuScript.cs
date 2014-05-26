using UnityEngine;
using System.Collections;

public class StartMenuScript : AMenuBase {

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
        //Screen.SetResolution(Screen.width,Screen.width, false);

        Time.timeScale = 1;

	}
	
    private bool LoadStage(string gameMode, string sceneName)
    {
        if(IsValidSelection(gameMode, sceneName))
        {
            GetComponent<GameModeBuilder>().LoadGameMode(gameMode);
            Application.LoadLevel(sceneName);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsValidSelection(string gameMode, string sceneName)
    {
        return (!string.IsNullOrEmpty(gameMode) && !string.IsNullOrEmpty(sceneName));
    }

   	protected override void SelectConfirmation()
	{
		//get GameMode
		string gameModeString =  SelectionGroups[0].GetComponent<SelectionGroup>().CurrentSelection as string;

		//get Stage
		GameObject StageUIElement = ((GameObject)SelectionGroups[1].GetComponent<SelectionGroup>().CurrentSelection) as GameObject;
		string sceneName = StageUIElement.GetComponent<StageInfo>().SceneName;

		if(!LoadStage(gameModeString, sceneName))
		{
			Debug.LogWarning("["+GetType().FullName+"] Invalid GameMode-Stage Selection");
		}
	}

    protected override void SelectCancel()
    {
    }

}
