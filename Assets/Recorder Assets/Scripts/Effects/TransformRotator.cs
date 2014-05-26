using UnityEngine;
using System.Collections;

public class TransformRotator : MonoBehaviour {

    private Quaternion startingRot;
    private float count = 0.0f;

    public float PitchStr;
    public float PitchSpeed;
    public float PitchTimingOffset;

    public float YawStr;
    public float YawSpeed;
    public float YawTimingOffset;

    public float RollStr;
    public float RollSpeed;
    public float RollTimingOffset;

	// Use this for initialization
	void Start () {
        startingRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
        count += Time.deltaTime;

        Quaternion newRot = transform.rotation;

        newRot.x = startingRot.x + Mathf.Sin(Time.time * PitchSpeed + PitchTimingOffset) * PitchStr;
        newRot.y = startingRot.y + Mathf.Sin(Time.time * YawSpeed + YawTimingOffset) * YawStr;
        newRot.z = startingRot.z + Mathf.Sin(Time.time * RollSpeed + RollTimingOffset) * RollStr;

        transform.rotation = newRot;
	}
}
