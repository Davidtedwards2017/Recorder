using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public bool bAcceptingInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(bAcceptingInput)
			MenuInput();
	}

	protected virtual void MenuInput()
	{
	}

	protected virtual bool ConfirmationInput()
	{
		return Input.GetKeyUp(KeyCode.Space) ||	Input.GetKeyUp(KeyCode.Return) || Input.GetButtonUp("Confirm_Button");
	}

	protected virtual bool CancelInput()
	{
		return Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Start_Button") || Input.GetButtonUp("Cancel_Button");
	}



}
