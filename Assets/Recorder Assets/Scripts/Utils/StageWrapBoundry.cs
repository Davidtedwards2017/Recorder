using UnityEngine;
using System.Collections;

public class StageWrapBoundry : MonoBehaviour {


    private BoxCollider bounds;
    public float MoveOffset;

	// Use this for initialization
	void Start () 
    {
	    bounds = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        Vector3 newPosition = other.transform.position;

        if(newPosition.x > Right)
        {
            newPosition.x = Left + MoveOffset;
        }
        else if(newPosition.x < Left)
        {
            newPosition.x = Right- MoveOffset;
        }

        if(newPosition.y > Top)
        {
            newPosition.y = Bottom + MoveOffset;
        }
        else if(newPosition.y < Bottom)
        {
            newPosition.y = Top - MoveOffset;
        }

        MoveGameObject(other.gameObject, newPosition);

    }

    float Right
    {
        get { return bounds.center.x + (bounds.size.x/2); }
    }
    float Left
    {
        get { return bounds.center.x - (bounds.size.x/2); }
    }

    float Top
    {
        get { return bounds.center.y + (bounds.size.y /2); }
    }
    float Bottom
    {
        get { return bounds.center.y - (bounds.size.y /2); }
    }

    private void MoveGameObject(GameObject go, Vector3 destination)
    {
        go.transform.position = destination;
    }
}
