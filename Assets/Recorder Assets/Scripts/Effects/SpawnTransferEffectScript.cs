using UnityEngine;
using System.Collections;

public class SpawnTransferEffectScript : MonoBehaviour {
	
	public float MinBendAmt;
	public float MaxBendAmt;

	public Transform EffectPrefab;
	public Transform Effect;

	//private float startTime;
	//private float distanceToTravel;

	private float currentStep;
	private float rate;
	private bool bStarted = false;
	public float t;
	//public SmoothQuaternion sr;

	private Transform[] points;
	
	public void StartEffect(Transform dest, float duration)
	{
		Vector3 vec = dest.position - transform.position;

		Vector3 left = (Vector3.Cross(vec.normalized, new Vector3(0,0,1))).normalized;
		Vector3 middle = transform.position + (vec.normalized * (vec.magnitude / 2));

		var startGo = new GameObject("travel node");
		startGo.transform.position = transform.position;

		var middleGo = new GameObject("travel node");
		float bendAmt = Random.Range(MinBendAmt, MaxBendAmt);
		middleGo.transform.position = middle + (left * bendAmt);

		points = new Transform[3];

		points[0] = startGo.transform;
		points[1] = middleGo.transform;
		points[2] = dest;

		Effect = Instantiate(EffectPrefab, transform.position, Quaternion.identity) as Transform;
		Effect.transform.parent = this.transform;
		
		currentStep = 0.0f;
		rate = 1.0f / duration;

		bStarted = true;

	}

	// Update is called once per frame
	void Update () {

		if(!bStarted)
			return;

		transform.position = Spline.InterpConstantSpeed(points, currentStep, EasingType.Sine, true, true);

		currentStep += Time.deltaTime * rate;

		if(currentStep >= 1)
			Destroy(gameObject);
	}

	void OnDestroy()
	{
        foreach(Transform t in points)
        {
            if(t != null && !t.tag.Equals("Station"))
            {
                Destroy(t.gameObject);
            }
        }   
	}


}
