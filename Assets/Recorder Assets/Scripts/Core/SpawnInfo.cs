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
	private bool StartedSpawnTimer = false;
	public float RespawnTime;
    public int Team;

	public PawnTypeEnum PawnSpawnType;
	public float baseLifeTime;


	void Awake()
	{
		station = GetComponent<Station>();
	}

	void Start () {
		StartSpawnSequence();
	}

	public bool StartSpawnSequence()
	{
		if(StartedSpawnTimer == true)
			return false;

		return StartedSpawnTimer = true;
	}

	// Update is called once per frame
	void Update () {

		if(!StartedSpawnTimer)
			return;

		if(RespawnTime > 0)
			RespawnTime -= Time.deltaTime;
		else
			AttemptSpawn();
	}

	void AttemptSpawn()
	{
        //PawnController spawnedPawn = station.SpawnPawn(Team, PawnSpawnType, baseLifeTime);
        //bool sucess = (spawnedPawn != null);
        //onSpawnCompleted(sucess, spawnedPawn);
        if(station.SpawnPawn(Team, PawnSpawnType, baseLifeTime) != null)
        {
		    Destroy(this);
        }
		
	}
}
