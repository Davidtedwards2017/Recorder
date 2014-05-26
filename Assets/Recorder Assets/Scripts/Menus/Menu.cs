using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public bool bEnabled;
	public MenuSelector Selector;
	public Transform CameraAnchor;

	#region event declaration
	public delegate void ElementSelectedEventHandler(UISelectableElement selectedElement);

	public event ElementSelectedEventHandler ElementSelectedEvent;

	void onElementSelected(){ if(ElementSelectedEvent != null) ElementSelectedEvent(Selector.CurrentSelection);}
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!bEnabled)
			return;
	}

	public void SetFocus(bool value)
	{
		if(Selector != null)
			Selector.SetEnabled(value);

		bEnabled = value;
	}

	public UISelectableElement GetSelectedItem()
	{
		return Selector.CurrentSelection;
	}

}
