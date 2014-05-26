using UnityEngine;
using System.Collections;

public class ModeSelectionGroup : SelectionGroup {

    private TextMesh m_GuiTextComponent;
    private int CurrentIndex = 0;

    public string[] GameModes;


    void Start()
    {
        m_GuiTextComponent = GetComponentInChildren<TextMesh>();
        SetSelection(GameModes[CurrentIndex]);
    }
        
    public override void SelectPrev()
    {
        CurrentIndex--;

        if(CurrentIndex < 0)
        {
            CurrentIndex = GameModes.Length - 1;
        }

        SetSelection(GameModes[CurrentIndex]);
    }

    public override void SelectNext()
    {
        CurrentIndex++;

        if(CurrentIndex > (GameModes.Length - 1))
        {
            CurrentIndex = 0;
        }
        
        SetSelection(GameModes[CurrentIndex]);
    }

    protected override void SetSelection(object obj)
    {
        base.SetSelection(obj);

        m_GuiTextComponent.text = (string)obj;
    }
}
