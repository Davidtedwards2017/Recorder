using UnityEngine;
using System.Collections;

public abstract class AMenuBase : MonoBehaviour {

    public GameObject[] SelectionGroups;
    public int CurrentSelectionIndex;
    public int CurrentSelectionGroupIndex;
    public float SelectionInputCooldown = 0.2f;

    public AudioClip sfxNavigation;
    public float sfxNavigationVolume;

    public AudioClip sfxConfirm;
    public float sfxConfirmVolume;

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

            AudioHelper.CreatePlayAudioObject(sfxNavigation,sfxNavigationVolume);
            
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
    
    protected virtual void SelectRight()
    {
    }
    
    protected virtual void SelectLeft()
    {
    }

    protected virtual void SelectConfirmation()
    {
        AudioHelper.CreatePlayAudioObject(sfxConfirm,sfxConfirmVolume);
    }

    protected virtual void SelectCancel()
    {
        //AudioHelper.CreatePlayAudioObject(sfxFire,sfxFireVolume);
    }

    private void SetSelectionGroupHighlight(GameObject selectionGroup, bool enable)
    {
        if(selectionGroup != null)
        {
            selectionGroup.GetComponent<SelectionGroup>().SetHighlight(enable);
        }
    }
    

}
