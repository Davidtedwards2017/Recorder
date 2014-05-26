using UnityEngine;
using System.Collections;

public class HueShifter : MonoBehaviour {

   
    public float speed = 1.0f;

    public float currentHue = 0;

	// Use this for initialization
	void Start () 
    {

        currentHue = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
         
        HSBColor hsb = HSBColor.FromColor(renderer.material.color);

        currentHue += Time.deltaTime * speed;

        if(currentHue > 1)
        {
            currentHue -= 1;
        }
        hsb.h = currentHue;

        renderer.material.color = HSBColor.ToColor(hsb);
    }
}
