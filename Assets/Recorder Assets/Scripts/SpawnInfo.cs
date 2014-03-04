using UnityEngine;
using System.Collections;
using PawnFramework;
using StageFramework;

[RequireComponent(typeof(Station))]
public class SpawnInfo : MonoBehaviour {

	#region event declaration
	public delegate void SpawnCompletedEventHandler(bool success, PawnController pawn);
	public event SpawnCompletedEventHandler SpawnCompletedEvent;
	
	void onSpawnCompleted(bool success, PawnController pawn) 
	{ 
		if(SpawnCompletedEvent != null) 
			SpawnCompletedEvent(success, pawn); 
	}
	#endregion
	private Station station;
	public bool bStart = false;
	public float RespawnTime;
	public PlayerInfo PlayerInfo;
	public PawnTypeEnum PawnSpawnType;
	public float baseLifeTime;


	void Awake()
	{
		station = GetComponent<Station>();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(!bStart)
			return;

		if(RespawnTime > 0)
			RespawnTime -= Time.deltaTime;
		else
			AttemptSpawn();
	}

	void AttemptSpawn()
	{
		PawnController spawnedPawn = station.SpawnPawn(PlayerInfo, PawnSpawnType, baseLifeTime);
		if( spawnedPawn != null)
		{
			onSpawnCompleted(true, spawnedPawn);
			Destroy(this);
		}
	}
}
