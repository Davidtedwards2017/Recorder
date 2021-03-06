﻿using UnityEngine;
using System.Collections;

public class Wiggle : MonoBehaviour {

    public float XStr = 1f;
    public float XSpeed = 1f;
    public float XtimingOffset = 0.0f;

    public float YStr = 1f;
    public float YSpeed = 1f;
    public float YtimingOffset = 0.0f;

    private float count;

    private Vector3 startingPosition;

    // Use this for initialization
	void Start () {
        startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        count += Time.deltaTime;

        Vector3 newPos = transform.position;
        newPos.x = startingPosition.x + Mathf.Sin(Time.time * XSpeed + XtimingOffset) * XStr;
        newPos.y = startingPosition.y + Mathf.Sin(Time.time * YSpeed + YtimingOffset) * YStr;

        transform.position = newPos;
	}
}
