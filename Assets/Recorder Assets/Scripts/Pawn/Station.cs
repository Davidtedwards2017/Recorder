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

    public bool CanSpawnHusks = false;

	public int TeamNumber;

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

	void onPawnSpawned(PawnController pawn)
	{
        /*
		if(PawnSpawnedEvent != null)
        {
            PawnSpawnedEvent(this, pawn);
        }

        PawnCtrl = pawn;
        gameInfo.CurrentGameMode.PawnSpawned(pawn);

        PlayerInfo playerInfo = GetComponentInChildren<PlayerInfo>();
        if( playerInfo != null)
        {
            playerInfo.AttachToPawn(pawn);
        }
        */
	}

	#endregion
	
	private GameInfo m_GameInfo;
	public float RecordingLifeTime;

	public Transform PawnPrefab;

	public Record StoredRecording;
	public PawnController PawnCtrl;

	public float RespawnTime;
	public float KillAwardAmount = 2f;
	public float AwardDecayPercent = 0.75f;

	public bool AutoSpawn = true;

	public GameInfo gameInfo
	{
		get { return m_GameInfo ?? (m_GameInfo = GameObject.FindGameObjectWithTag("GameInfo").GetComponent<GameInfo>()); }
	}
	public SpawnInfo spawnInfo
	{
		get{ return GetComponent<SpawnInfo>(); }
	}
	
    public bool AddNewSpawnInfo(PawnTypeEnum pawnType, int team, float RespawnTime)
	{
		SpawnInfo spawnInfo = gameObject.AddComponent<SpawnInfo>();
		
		spawnInfo.RespawnTime = RespawnTime;
		spawnInfo.baseLifeTime = gameInfo.CurrentGameMode.BaseLifeTime;
		spawnInfo.PawnSpawnType = pawnType;

        spawnInfo.Team = (GameInfo.Instance.CurrentGameMode.UseTeamSpawns)? team : GameInfo.Instance.CurrentGameMode.GetTeamFromPawnType(pawnType);
     		
		//spawnInfo.SpawnCompletedEvent += onSpawnCompleted;

		return true;
	}

	public PawnTypeEnum GetPawnType()
	{
		if(PawnCtrl == null)
			return PawnTypeEnum.NONE;

		return PawnCtrl.GetPawnType();
	}

    void Update()
    {
        if(AutoSpawn && PawnCtrl == null)
        {
            QueueSpawn();
        }
    }

	public void QueueSpawn()
	{
		if(spawnInfo != null)
			return;

        if( CanSpawnHusks && StoredRecording == null) 
		{
            AddNewSpawnInfo(PawnTypeEnum.HUSK, TeamNumber, gameInfo.CurrentGameMode.mimicRespawnTime);
		}
        else if(StoredRecording != null)
		{
            AddNewSpawnInfo(PawnTypeEnum.MIMIC, TeamNumber, gameInfo.CurrentGameMode.mimicRespawnTime);
		}
	}

	public bool QueueDriverSpawn(PlayerInfo playerInfo, float spawnDelay)
	{
		if(spawnInfo == null) //create new spawn info if one doesnt already exist
		{
			return AddNewSpawnInfo(PawnTypeEnum.DRIVER, playerInfo.team, spawnDelay);
		}
		else if(spawnInfo.PawnSpawnType != PawnTypeEnum.DRIVER) //dont override another Driver waiting to spawn
		{
			spawnInfo.PawnSpawnType = PawnTypeEnum.DRIVER;
            spawnInfo.Team = playerInfo.team;
			spawnInfo.RespawnTime = spawnDelay;

			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void StopSpawning()
	{
		SpawnInfo spawnInfo = GetComponent<SpawnInfo>();

        AutoSpawn = false;

		if(spawnInfo != null)
			Destroy(spawnInfo);
	}

	public PawnController SpawnPawn(int team, PawnTypeEnum pawnType, float lifeTime)
	{
		//despawn existing pawn if new one spawnss
		DespawnExisitingPawn();

		Transform T = Instantiate(PawnPrefab, gameObject.transform.position, Quaternion.identity) as Transform;
		PawnController pawn = T.gameObject.GetComponent<PawnController>();

        pawn.station = this;
        pawn.PawnDeathEvent += onPawnDeath;
        pawn.LifeTimeExausedEvent += onLifeTimeExausted;

		if(pawnType == PawnTypeEnum.DRIVER)
		{
            pawn.InitiateForPawnType<Driver>();
            ((Driver)pawn.pawnType).EndLifeTime = lifeTime;
        }
		else if(pawnType == PawnTypeEnum.MIMIC)
		{
            pawn.InitiateForPawnType<Mimic>();
            pawn.Recording = StoredRecording;
            //pawn.EndLifeTime = RecordingLifeTime;

		}
        else
        {
            pawn.InitiateForPawnType<Husk>();
        }

        pawn.SetTeam(team);

        gameInfo.CurrentGameMode.PawnSpawned(pawn);
        
        PlayerInfo playerInfo = GetComponentInChildren<PlayerInfo>();
        if( playerInfo != null)
        {
            playerInfo.AttachToPawn(pawn);
        }

        PawnCtrl = pawn;


        return pawn;
	}
	                                
	private void RewardForKill(PawnController pawn, float amt )
	{
				
		if(pawn.killCount > 0)
			amt *= (AwardDecayPercent / pawn.killCount);
		
		pawn.AddTime(KillAwardAmount);
		
		pawn.killCount += 1;
	}

	private void DespawnExisitingPawn()
	{
		if(PawnCtrl == null)
			return;

		PawnCtrl.LifeTimeExausedEvent -= onLifeTimeExausted;
		PawnCtrl.PawnDeathEvent -= onPawnDeath;

		Destroy(PawnCtrl.gameObject);
	}

	#region events
	void onLifeTimeExausted(PawnController pawn)
	{
        StoredRecording = pawn.Recording;

        if(pawn.GetPawnType() != PawnTypeEnum.MIMIC)
        {
            //RecordingLifeTime = pawn.CurrentLifeTime;
        }

		onPawnLifeTimeOver(pawn);
	}

	void onPawnDeath(PawnController pawn, PawnController destroyedBy)
	{
		StoredRecording = pawn.Recording;

		if(pawn.GetPawnType() != PawnTypeEnum.MIMIC)
        {
		//	RecordingLifeTime = pawn.CurrentLifeTime;
        }

		if(destroyedBy != null && destroyedBy.GetPawnType() == PawnTypeEnum.DRIVER)
			RewardForKill(destroyedBy, KillAwardAmount);

		//QueueSpawn();
		
		pawn.PawnDeathEvent -= onPawnDeath;
		pawn.LifeTimeExausedEvent -= onLifeTimeExausted;

        onPawnDestroyed(pawn, destroyedBy);
	}

	void onSpawnCompleted(bool success, PawnController pawn)
	{/*
		if(success)
		{
			PawnCtrl = pawn;
			gameInfo.CurrentGameMode.PawnSpawned(pawn);
            onPawnSpawned(pawn);
		}
  */      
	}



	#endregion
}
