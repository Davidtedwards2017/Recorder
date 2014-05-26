using UnityEngine;
using System.Collections;
using PawnFramework;
using StageFramework;

public class PlayerInfo : MonoBehaviour {

	#region Event declaration
	public delegate void KilledEnemyEventHandler(PlayerInfo player);
	public event KilledEnemyEventHandler KilledEnemyEvent;
	
	void onKilledEnemy()
	{
		if(KilledEnemyEvent != null)
			KilledEnemyEvent(this);
	}
	#endregion

	private ArrayList AvailableSpawns = new ArrayList();
	private object SpawnHandle = new object();

	private Vector3 PrevPawnLocation;

    public PawnController currentPawn;
	public enum PlayerStateEnum { UNATTACHED, QUEUED, ATTACHED }

	public int team;
	public int playerNum;
	public Station currentStation;

    public static PlayerInfo CreatePlayer(int player, int team)
    {
        GameObject go = new GameObject("Player_" + player);
        PlayerInfo playerInfo = go.AddComponent<PlayerInfo>() as PlayerInfo;
        playerInfo.InstantiatePlayerInfo(player, team);
        return playerInfo;
    }

	public void InstantiatePlayerInfo(int player, int team)
	{
		this.playerNum = player;
		this.team = team;
	}

	public bool TryToSpawn()
	{
		bool retval;

		lock(SpawnHandle)
		{
		    Station station = GetNextPlayerStationSpawn();

			if(station.QueueDriverSpawn(this, GameInfo.Instance.CurrentGameMode.playerRespawnTime))
			{
				gameObject.MoveTo(station.gameObject);

				currentStation = station;

				currentStation.PawnLifeTimeOverEvent += onPawnLifeTimeOver;
				currentStation.PawnDestroyedEvent += onPawnDestroyed;
                //currentStation.PawnSpawnedEvent += onPawnSpawned;

				retval = true;

				SpawnTransferEffect(PrevPawnLocation, station.transform, GameInfo.Instance.CurrentGameMode.playerRespawnTime);
			}
			else
			{
				retval = false;
			}
		}
		return retval;
	}

	public void KilledPawn(PawnController killedPawn)
	{
		if(killedPawn != null)
			onKilledEnemy();
	}

	private void SpawnTransferEffect(Vector3 startingPos, Transform dest, float duration)
	{
		GameObject go = Instantiate(Resources.Load<GameObject>("SpawnTransferEffectPrefab"), startingPos, Quaternion.identity) as GameObject;

       	SpawnTransferEffectScript script = go.GetComponent<SpawnTransferEffectScript>();
        TeamInfo.AddTeamInfo(go, team);

		script.StartEffect(dest, duration);
	}
   
	private void Detach()
	{
        if(currentStation != null)
        {
            //currentStation.PawnSpawnedEvent -= onPawnSpawned;
            currentStation.PawnDestroyedEvent -= onPawnDestroyed;
            currentStation.PawnLifeTimeOverEvent -= onPawnLifeTimeOver;
        }
        else
        {
            //make sure this player is not listenting for any other Pawn Event
            foreach(Station s in GameInfo.Instance.GetStations())
            {
                //s.PawnSpawnedEvent -= onPawnSpawned;
                s.PawnDestroyedEvent -= onPawnDestroyed;
                s.PawnLifeTimeOverEvent -= onPawnLifeTimeOver;
            }
        }

        currentStation = null;
        currentPawn = null;

        //gameObject.MoveTo(GameInfo.Instance.SpawnQueue);
	}

	private Station GetNextPlayerStationSpawn()
	{
		if(AvailableSpawns == null || AvailableSpawns.Count <= 0)
			AvailableSpawns.AddRange(GetStationsToSpawnAt());
		
		int index = Random.Range(0,AvailableSpawns.Count);
		
		Station station = AvailableSpawns[index] as Station;
		AvailableSpawns.RemoveAt(index);
		
		return station;
	}

	private Station[] GetStationsToSpawnAt()
	{
		if(GameInfo.Instance.CurrentGameMode.UseTeamSpawns)
		{
			return GameInfo.Instance.GetStations(team);
		}
		else
		{
			return GameInfo.Instance.GetStations();
		}
	}

    public void AttachToPawn(PawnController pawn)
    {        
        gameObject.MoveTo(pawn.gameObject);
        currentPawn = pawn;
    }

	#region event listeners

	void onPawnLifeTimeOver(Station station, PawnController pawn)
	{
		Detach();

		PrevPawnLocation = pawn.transform.position;
	}
	void onPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy)
	{
		Detach();

		PrevPawnLocation = pawn.transform.position;
	}
/*
    void onPawnSpawned(Station station, PawnController pawnCtrl)
    {
        gameObject.MoveTo(pawnCtrl.gameObject);
        currentPawn = pawnCtrl;

        if(currentStation != null)
        {
            currentStation.PawnSpawnedEvent -= onPawnSpawned;
        }
        else
        {
            //make sure this player is not listenting for any other PawnSpawnEvents
            foreach(Station s in GameInfo.Instance.GetStations())
            {
                s.PawnSpawnedEvent -= onPawnSpawned;
            }
        }
    }
*/
	#endregion


}
