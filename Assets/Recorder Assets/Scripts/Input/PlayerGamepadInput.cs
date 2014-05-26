using UnityEngine;
using System.Collections;

public class PlayerGamepadInput : MonoBehaviour {
	
	//player 1 Input mapping strings
	private const string P1_HORIZONTAL = "P1_Horizontal";
	private const string P1_VERTICAL = "P1_Vertical";
	private const string P1_FIRE = "P1_Fire";
	private const string P1_JUMP = "P1_Jump";
	private const string P1_START = "StartButton";

	//player 2 Input mapping strings
	private const string P2_HORIZONTAL = "P2_Horizontal";
	private const string P2_VERTICAL = "P2_Vertical";
	private const string P2_FIRE = "P2_Fire";
	private const string P2_JUMP = "P2_Jump";
	private const string P2_START = "StartButton";

	public static Vector3 GetInputDirection(int player)
	{
		return new Vector3(Input.GetAxis(GetHorizontalAxisInputString(player)), Input.GetAxis(GetVerticalAxisInputString(player)), 0);
	}

    public static bool GetDirectionRelease()
    {
        return (
            Input.GetAxis(GetVerticalAxisInputString(1)) == 0 && Input.GetAxis(GetHorizontalAxisInputString(1)) == 0 &&
            Input.GetAxis(GetVerticalAxisInputString(2)) == 0 && Input.GetAxis(GetHorizontalAxisInputString(2)) == 0 );
    }

    public static bool GetStartButtonInput()
    {
        return Input.GetButtonUp("Start_Button");
    }

    public static bool GetConfirmationInput()
    {
        return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetButtonUp("Confirm_Button");
    }

    public static bool GetCancelInput()
    {
        return Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Cancel_Button");
    }

	public static bool GetJump(int player)
	{
		return Input.GetButton(GetJumpInputString(player));
	}
	public static bool GetFire(int player)
	{
		return Input.GetButton(GetFireInputString(player));
	}

	public static bool GetUpInput()
	{
		return (Input.GetKey("up") || 
		        Input.GetAxis(GetVerticalAxisInputString(1)) > 0 ||
		        Input.GetAxis(GetVerticalAxisInputString(2)) > 0 );
	}

	public static bool GetUpInput(int player)
	{
		return Input.GetAxis(GetVerticalAxisInputString(player)) > 0;
	}

	public static bool GetDownInput()
	{
		return (Input.GetKey ("down") || 
						GetDownInput (1) ||
						GetDownInput (2));
	}

	public static bool GetDownInput(int player)
	{
		return Input.GetAxis(GetVerticalAxisInputString(player)) < 0;
	}

	public static bool GetLeftInput()
	{
		return (Input.GetKey("left") || 
		        GetLeftInput(1) ||
		        GetLeftInput(2));
	}

	public static bool GetLeftInput(int player)
	{
		return Input.GetAxis(GetHorizontalAxisInputString(player)) < 0;
	}

	public static bool GetRightInput()
	{
		return (Input.GetKey ("right") || 
						GetRightInput (1) ||
						GetRightInput (2));
	}

	public static bool GetRightInput(int player)
	{
		return Input.GetAxis(GetHorizontalAxisInputString(player)) > 0;
	}

	private static string GetHorizontalAxisInputString(int player)
	{
		if (player == 1) 
		{
			return P1_HORIZONTAL;
		} 
		else if (player == 2) 
		{
			return P2_HORIZONTAL;
		}
		
		return null;
	}
	private static string GetVerticalAxisInputString(int player)
	{
		if (player == 1) 
		{
			return P1_VERTICAL;
		} 
		else if (player == 2) 
		{
			return P2_VERTICAL;
		}
		
		return null;
	}
	private static string GetJumpInputString(int player)
	{
		if (player == 1) 
		{
			return P1_JUMP;
		} 
		else if (player == 2) 
		{
			return P2_JUMP;
		}
		
		return null;
	}
	private static string GetFireInputString(int player)
	{
		if (player == 1) 
		{
			return P1_FIRE;
		} 
		else if (player == 2) 
		{
			return P2_FIRE;
		}

		return null;
	}

}
