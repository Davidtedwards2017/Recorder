using UnityEngine;
using System.Collections;
using StageFramework;

public class EndScreenMenu : AMenuBase {

	protected override void Start () 
    {
        base.Start();
        Time.timeScale = 0;
    }

    void OnDestroy()
    {
        Time.timeScale = 1;
    }
	
    protected override void SelectConfirmation()
    {
        //get GameMode
        string currentSelected =  SelectionGroups[CurrentSelectionGroupIndex].name;
        
        switch(currentSelected)
        {
            case "Quit":
                QuitOption();
                break;
            case "Rematch":
                GameInfo.Instance.CurrentGameMode.RestartGame();
                break;
            default:
                break;

        } 
        Destroy(transform.root.gameObject);
    }


    private void QuitOption()
    {
        Debug.Log("Quit Selected");
        Application.LoadLevel("StartScreen");
    }
}
