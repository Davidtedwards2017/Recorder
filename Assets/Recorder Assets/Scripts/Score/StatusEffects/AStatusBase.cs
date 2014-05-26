using UnityEngine;
using System.Collections;

public abstract class AStatusBase : MonoBehaviour {

    public float StatusDuration;
    public PawnController pawn;
	// Use this for initialization
	void Start () {

        pawn = GetComponent<PawnController>();

        if(pawn == null)
        {
            Debug.LogError("["+GetType().FullName+"] Failed to Start status, unable to retrive PawnController.");
            Component.Destroy(this);
        }

        StartStatus();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
	    StatusDuration -= Time.deltaTime;

        if(StatusDuration <= 0)
        {
            StopStatus();
            Component.Destroy(this);
        }
	}

    protected abstract void StartStatus();
    protected abstract void StopStatus();
}
