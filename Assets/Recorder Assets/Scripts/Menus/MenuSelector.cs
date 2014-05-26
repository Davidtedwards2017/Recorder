using UnityEngine;
using System.Collections;

public class MenuSelector : TimeScaleIndpendentUpdate {

	public bool bEnabled;

	public float SelectionSpeed;
	public float SelectionCooldown = 0.2f;

	public UISelectableElement CurrentSelection;

	public float X_Offset;
	public float Y_Offset;
	public float Z_Offset;

	private float m_SelectCD;

	public void SetEnabled(bool value)
	{
		bEnabled = value;
	}
	// Update is called once per frame
	protected override void Update () {

		base.Update();

		if(m_SelectCD > 0)
			m_SelectCD -= deltaTime;
			
		if(!bEnabled)
			return;

		UpdateSelection();

        Vector3 targetPosition;

        if(CurrentSelection.SelectionAnchor != null)
        {
            targetPosition = CurrentSelection.SelectionAnchor.transform.position;
        }
        else
        {
		    targetPosition = CurrentSelection.transform.position;
        }

		if(transform.position != targetPosition)
        {
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, SelectionSpeed * deltaTime);
        }
	}
	
	public void UpdateSelection()
	{
		if(m_SelectCD > 0)
			return;

		if(InputUp())
		{
			SelectUp();
		}
		else if(InputDown())
		{
			SelectDown();
		}
		else if(InputLeft())
		{
			SelectLeft();
		}
		else if(InputRight())
		{
			SelectRight();
		}
	}

	private bool InputUp()
	{
		return PlayerGamepadInput.GetUpInput();
	}

	private bool InputDown()
	{
		return PlayerGamepadInput.GetDownInput();
	}

	private bool InputLeft()
	{
		return PlayerGamepadInput.GetLeftInput();
	}
	
	private bool InputRight()
	{
		return PlayerGamepadInput.GetRightInput();
	}

	void SelectUp()
	{
		Select(CurrentSelection.Up);
	}

	void SelectDown()
	{
		Select(CurrentSelection.Down);
	}

	void SelectLeft()
	{
		Select(CurrentSelection.Left);
	}

	void SelectRight()
	{
		Select(CurrentSelection.Right);
	}

	public void Select(Transform newSelection)
	{
		if(newSelection == null)
			return;

     	UISelectableElement element = newSelection.GetComponent<UISelectableElement>();

		if(element == null)
		{
			Debug.LogWarning("["+GetType()+"]" + newSelection.name +" is not a UISelectableElement");
			return;
		}

		CurrentSelection = element;

		m_SelectCD = SelectionCooldown;
	}
	
	Vector3 GetOffsetVector(Vector3 vec, float x, float y, float z)
	{
		vec.x += x;
		vec.y += y;
		vec.z += z;
		return vec;
	}
}
