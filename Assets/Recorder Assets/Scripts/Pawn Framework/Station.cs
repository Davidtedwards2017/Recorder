using UnityEngine;
using System.Collections;
using PawnFramework;
using StageFramework;


public class Station : MonoBehaviour {


	#region event declarations

	public delegate void PawnLifeTimeOverEventHandler(Station station, PawnController pawn);
	public delegate void PawnDestroyedEventHandler(Station station, PawnController pawn, PawnController destroyedBy);
	public delegate void PawnSpawnedEventHandler(Station station, PawnController pawnCtrl);

	public PawnLifeTimeOverEventHandler PawnLifeTimeOverEvent;
	public PawnDestroyedEventHandler PawnDestroyedEvent;
	public PawnSpawnedEventHandler PawnSpawnedEvent;

	void onPawnLifeTimeOver(PawnController pawn)
	{
		if(PawnLifeTimeOverEvent != null)
			PawnLifeTimeOverEvent(this, pawn);
	}

	void onPawnDestroyed(PawnController pawn, PawnController destroyedBy)
	{
		if(PawnDestroyedEvent!= null)
			PawnDestroyedEvent(this, pawn, destroyedBy);
	}

	void onPawnSpawned(PawnController pawnCtrl)
	{
		if(PawnSpawnedEvent != null)
			PawnSpawnedEvent(this, pawnCtrl);
	}

	#endregion
	
	private GameInfo gameInfo;
	public float RecordingLifeTime;

	public Transform PawnPrefab;
	public Transform TimeBombPrefab;

	public Record StoredRecording;
	public PawnController PawnCtrl;

	public float RespawnTime;
	public float KillAwardAmount = 2f;
	public float AwardDecayPercent = 0.75f;

	public bool AutoSpawn = true;

	public SpawnInfo spawnInfo
	{
		get{ return GetComponent<SpawnInfo>(); }
	}
	
	// Use this for initialization
	void Start () {
		gameInfo = GameObject.FindGameObjectWithTag("GameInfo").GetComponent<GameInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(AutoSpawn && (spawnInfo == null))
		   AddNewSpawnInfo();

		if(PawnCtrl == null && spawnInfo != null && !spawnInfo.bStart)
			spawnInfo.bStart = true;

	}

	public void AddNewSpawnInfo()
	{
		SpawnInfo spawnInfo = gameObject.AddComponent<SpawnInfo>();
		
		spawnInfo.RespawnTime = RespawnTime;
		spawnInfo.baseLifeTime = gameInfo.CurrentGameMode.BaseLifeTime;
		spawnInfo.PawnSpawnType = PawnTypeEnum.HUSK;
		spawnInfo.PlayerInfo = gameInfo.CurrentGameMode.player0;
		spawnInfo.SpawnCompletedEvent += onSpawnCompleted;
	}

	public PawnTypeEnum GetPawnType()
	{
		if(PawnCtrl == null)
			return PawnTypeEnum.NONE;

		return PawnCtrl.GetPawnType();
	}

	public bool QueueDriverSpawn(PlayerInfo playerInfo)
	{
		bool retval;
		//Station NextStation = GetNextStationSpawn();
		
		if(GetPawnType() == PawnTypeEnum.HUSK)
		{
			PawnController pawnCtrl = PossessExistingHusk(playerInfo);
			
			if(pawnCtrl == null)
			{
				Debug.LogWarning("[QueueDriverSpawn] failed to Possess husk");
				return false;
			}
			
			pawnCtrl.SetPlayable(true);
			retval = true;
		}
		else
		{
			retval = OverrideStationSpawn( playerInfo, PawnTypeEnum.DRIVER);
		}


		return retval;
	}
	
	public bool OverrideStationSpawn(PlayerInfo playerInfo, PawnTypeEnum pawnType)
	{
		if(spawnInfo == null)
		{
			Debug.LogWarning("[OverrideNextStationSpawn] " + this + " spawnInfo not set");
			return false;
		}
		if(spawnInfo.PawnSpawnType == PawnTypeEnum.DRIVER)
		{
			//dont override another Drivers spawn
			return false;
		}
		
		spawnInfo.PawnSpawnType = pawnType;
		spawnInfo.PlayerInfo = playerInfo;
		spawnInfo.SpawnCompletedEvent += onSpawnCompleted;

		return true;
	}

	public PawnController SpawnPawn(PlayerInfo pawnInfo, PawnTypeEnum pawnType, float lifeTime)
	{
		Transform T = Instantiate(PawnPrefab, gameObject.transform.position, Quaternion.identity) as Transform;
		PawnCtrl = T.gameObject.GetComponent<PawnController>();

		PawnCtrl.ChangePawnType(pawnType);

		PawnCtrl.station = this;
		PawnCtrl.PawnDeathEvent += onPawnDeath;
		PawnCtrl.LifeTimeExausedEvent += onLifeTimeExausted;

		if(pawnType == PawnTypeEnum.DRIVER)
		{
			PawnCtrl.EndLifeTime = lifeTime;
		}
		else if(pawnType == PawnTypeEnum.MIMIC)
		{
			PawnCtrl.Recording = StoredRecording;
			PawnCtrl.EndLifeTime = RecordingLifeTime;
		}

		PawnCtrl.PossessPawn(pawnInfo);

		return PawnCtrl;
	}

	public PawnController PossessExistingHusk(PlayerInfo playerInfo)
	{
		if( PawnCtrl == null)
			return null;

		PawnCtrl.ChangePawnType(PawnTypeEnum.DRIVER);
		PawnCtrl.PossessPawn(playerInfo);
		PawnCtrl.EndLifeTime = PawnCtrl.CurrentLifeTime + gameInfo.CurrentGameMode.BaseLifeTime;

		return PawnCtrl;
	}
	                                
	public void ChangePawnType(PlayerInfo pawnInfo, PawnTypeEnum pawnType)
	{
		PawnCtrl.ChangePawnType(pawnType);
	}

	public TimeBomb SpawnTimeBomb(float lifeTime)
	{
		Transform T = Instantiate(TimeBombPrefab, gameObject.transform.position, Quaternion.identity) as Transform;

		TimeBomb bomb = T.GetComponent<TimeBomb>();
		bomb.LifeTime = lifeTime;

		return bomb;
	}

	#region events
	void onLifeTimeExausted(PawnController pawn)
	{
		onPawnLifeTimeOver(pawn);
	}

	void onPawnDeath(PawnController pawn, PawnController destroyedBy)
	{
		StoredRecording = pawn.Recording;

		if(pawn.GetPawnType() != PawnTypeEnum.MIMIC)
			RecordingLifeTime = pawn.CurrentLifeTime;

		onPawnDestroyed(pawn, destroyedBy);

		if(destroyedBy != null)
		{
			float amt = KillAwardAmount;

			if(pawn.killCount > 0)
				amt *= (AwardDecayPercent / pawn.killCount);

			destroyedBy.AddTime(KillAwardAmount);

			destroyedBy.killCount += 1;
		}

		pawn.PawnDeathEvent -= onPawnDeath;
		pawn.LifeTimeExausedEvent -= onLifeTimeExausted;
	}

	void onSpawnCompleted(bool success, PawnController pawn)
	{
		if(success)
		{
			PawnCtrl = pawn;
			gameInfo.CurrentGameMode.PawnSpawned(pawn);
		}
	}

	#endregion
}
