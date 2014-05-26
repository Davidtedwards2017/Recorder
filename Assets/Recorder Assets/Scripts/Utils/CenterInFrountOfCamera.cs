using UnityEngine;
using System.Collections;

public class CenterInFrountOfCamera : MonoBehaviour {

    public float DistanceFromCamera;
    public Vector3 TransformOffset;
	
    // Use this for initialization
	void Start () {
	
        float x = Screen.width / 2;
        float y = Screen.height /2;
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x,y));
        
        transform.position = (ray.direction * DistanceFromCamera) + Camera.main.transform.position + TransformOffset;
	}

}
