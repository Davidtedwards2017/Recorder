using UnityEngine;
using System.Collections;

public class TimeBomb : MonoBehaviour {

	private bool bRunning;
	public float LifeTime;

	#region Event decaration
	public delegate void DetonationEventHandler();
	public event DetonationEventHandler DetonationEvent;
	void onDetonation() 
	{ 
		if(DetonationEvent != null) 
			DetonationEvent(); 
	}

	#endregion
	void Awake()
	{
		bRunning = false;
	}

	public void SetActive(bool active)
	{
		bRunning = active;
	}

	void LateUpdate()
	{
		if(!bRunning)
			return;

		LifeTime -= Time.deltaTime;
		if(LifeTime <= 0)
			Detonate();
	}

	void Detonate()
	{
		onDetonation();
		Destroy(gameObject);
	}
}
