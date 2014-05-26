using UnityEngine;
using System.Collections;

public class StageSelectionGroup : SelectionGroup {

    private int CurrentIndex = 0;

    public GameObject[] StageUIElements;

	// Use this for initialization
	void Start () 
    {
        foreach(GameObject UIElement in StageUIElements)
        {
            SetElementVisable(UIElement, false);
        }

        SetSelection(StageUIElements[CurrentIndex]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
   
    public override void SelectPrev()
    {
        CurrentIndex--;

        if(CurrentIndex < 0)
        {
            CurrentIndex = StageUIElements.Length - 1;
        }

        SetSelection(StageUIElements[CurrentIndex]);
    }

    public override void SelectNext()
    {
        CurrentIndex++;
        
        if(CurrentIndex > (StageUIElements.Length - 1))
        {
            CurrentIndex = 0;
        }
        
        SetSelection(StageUIElements[CurrentIndex]);
    }

    private void SetElementVisable(GameObject go, bool visable)
    {
        go.SetActive(visable);
    }

    protected override void SetSelection(object obj)
    {
        if(CurrentSelection != null)
        {
            SetElementVisable((GameObject)CurrentSelection, false);
        }

        base.SetSelection(obj);

        SetElementVisable((GameObject)CurrentSelection, true);
    }

}
