using UnityEngine;
using System.Collections;

public abstract class AMenuBase : MonoBehaviour {

    public GameObject[] SelectionGroups;
    public int CurrentSelectionIndex;
    public int CurrentSelectionGroupIndex;
    public float SelectionInputCooldown = 0.2f;

    private float m_SelectCD;
    private GameObject m_CurrentSelectionGroup;
    
    public GameObject CurrentSelectionGroup
    {
        get{ return m_CurrentSelectionGroup; }
        set 
        {
            if(m_CurrentSelectionGroup == value)
            {
                return;
            }
            
            SetSelectionGroupHighlight(m_CurrentSelectionGroup, false);
            SetSelectionGroupHighlight(value, true);
            
            m_CurrentSelectionGroup = value;
        }
    }

	// Use this for initialization
	protected virtual void Start () 
    {
        foreach( GameObject go in SelectionGroups)
        {
            SetSelectionGroupHighlight(go, false);
        }
        
        CurrentSelectionGroup = SelectionGroups[CurrentSelectionGroupIndex = 0];
	}
	
    // Update is called once per frame
    protected virtual void Update () 
    {
        if(PlayerGamepadInput.GetDirectionRelease())
        {
            m_SelectCD = 0;
        }
        
        if(m_SelectCD <= 0)
        {
            UpdateSelection();
        }
        else
        {
            m_SelectCD -= Time.deltaTime;
        }
    }

    public void UpdateSelection()
    {         
        if(PlayerGamepadInput.GetUpInput())
        {
            SelectUp();
            m_SelectCD = SelectionInputCooldown;
        }
        else if(PlayerGamepadInput.GetDownInput())
        {
            SelectDown();
            m_SelectCD = SelectionInputCooldown;
        }
        else if(PlayerGamepadInput.GetLeftInput())
        {
            SelectLeft();
            m_SelectCD = SelectionInputCooldown;
        }
        else if(PlayerGamepadInput.GetRightInput())
        {
            SelectRight();
            m_SelectCD = SelectionInputCooldown;
        }
        else if(PlayerGamepadInput.GetConfirmationInput())
        {
            SelectConfirmation();            
        }
        else if(PlayerGamepadInput.GetCancelInput())
        {
            SelectCancel();
        }
    }

    private void SelectUp()
    {
        CurrentSelectionGroupIndex--;
        
        if(CurrentSelectionGroupIndex < 0)
        {
            CurrentSelectionGroupIndex = SelectionGroups.Length - 1;
        }
        
        CurrentSelectionGroup = SelectionGroups[CurrentSelectionGroupIndex];
    }
    
    private void SelectDown()
    {
        CurrentSelectionGroupIndex++;
        
        if(CurrentSelectionGroupIndex > (SelectionGroups.Length - 1))
        {
            CurrentSelectionGroupIndex = 0;
        }
        
        CurrentSelectionGroup = SelectionGroups[CurrentSelectionGroupIndex];
    }
    
    private void SelectRight()
    {
        CurrentSelectionGroup.GetComponent<SelectionGroup>().SelectNext();
    }
    
    private void SelectLeft()
    {
        CurrentSelectionGroup.GetComponent<SelectionGroup>().SelectPrev();
    }

    protected abstract void SelectConfirmation();
    protected abstract void SelectCancel();

    private void SetSelectionGroupHighlight(GameObject selectionGroup, bool enable)
    {
        if(selectionGroup != null)
        {
            selectionGroup.GetComponent<SelectionGroup>().SetHighlight(enable);
        }
    }
    

}
