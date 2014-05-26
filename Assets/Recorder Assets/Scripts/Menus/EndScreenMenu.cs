using UnityEngine;
using System.Collections;
using StageFramework;

public class EndScreenMenu : MenuScript {


    public MenuSelector Selector;
	// Use this for initialization
	void Start () {
        //WinnerTextMesh = transform.FindChild("Winning Player").gameObject.GetComponent<tk2dTextMesh>();
	}
	
    private void DisplayWinnerInfo()
    {
        //PlayerInfo winningPlayer = PlayerInfo.CreatePlayer(3,1);
        //int Score = 2032321;
        //WinnerTextMesh.text = winningPlayer.gameObject.name;
        //WinnerTextMesh.color = GameInfo.GetTeamColor(winningPlayer.team);
     }


    protected override void MenuInput()
    {
        if(ConfirmationInput())
        {
            //get current selection
            switch(Selector.CurrentSelection.name)
            {
                case "Rematch":
                    RematchOption();
                    break;
                case "Quit":
                    QuitOption();
                    break;
                default:
                    break;
            }
        }
    }
    
    private void RematchOption()
    {
        Debug.Log("Rematch Selected");
        GameStateManager.Instance.ReadyToStartRound();

        Destroy(gameObject);
    }

    private void QuitOption()
    {
        Debug.Log("Quit Selected");
        Application.LoadLevel("StartScreen");
    }
}
