using UnityEngine;
using System.Collections;

public class SelectionGroup : MonoBehaviour {

    public object CurrentSelection;

    public virtual void SetHighlight(bool value)
    {
        m_LeftPointer.enabled = value;
        m_RightPointer.enabled = value;
    }

    public virtual void SelectPrev()
    {
    }

    public virtual void SelectNext()
    {
    }

    protected SpriteRenderer m_LeftPointer;
    protected SpriteRenderer m_RightPointer;

    protected virtual void Awake()
    {
        m_LeftPointer = transform.FindChild("LeftPointer").GetComponent<SpriteRenderer>();
        m_RightPointer = transform.FindChild("RightPointer").GetComponent<SpriteRenderer>();
    }

    protected virtual void SetSelection(object obj)
    {
        if(CurrentSelection == obj)
        {
            return;
        }

        CurrentSelection = obj;    
    }

}
