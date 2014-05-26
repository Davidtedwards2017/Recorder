using UnityEngine;
using System.Collections;

public class HideOnPause : MonoBehaviour {

	
	// Update is called once per frame
	void Update () 
    {
	    if(Time.deltaTime == 0 && renderer.enabled )
        {
            renderer.enabled = false;
        }
        else if(Time.deltaTime != 0 && !renderer.enabled)
        {
            renderer.enabled = true;
        }
	}
}
