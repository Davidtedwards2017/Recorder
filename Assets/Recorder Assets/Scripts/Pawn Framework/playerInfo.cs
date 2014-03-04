using UnityEngine;
using System.Collections;
using PawnFramework;
using StageFramework;

public class PlayerInfo {

	private ArrayList AvailableSpawns = new ArrayList();
	private object SpawnHandle = new object();
	private GameInfo gameInfo;

	public enum PlayerStateEnum { UNATTACHED, ATTACHED }


	public int team;
	//player = 0 is not a player
	public int player;
	public Station currentStation;

	public PlayerStateEnum playerState;



	public PlayerInfo(int player, int team, GameInfo gameInfo)
	{
		this.player = player;
		this.team = team;
		this.gameInfo = gameInfo;
		playerState = PlayerStateEnum.UNATTACHED;
	}
	

	public bool TryToSpawn()
	{
		bool retval;

		lock(SpawnHandle)
		{
			if(ShouldSpawn())
			{
				Station station = GetNextPlayerStationSpawn();

				if(station.QueueDriverSpawn(this))
				{
					playerState = PlayerStateEnum.ATTACHED;
					currentStation = station;

					currentStation.PawnLifeTimeOverEvent += onPawnLifeTimeOver;
					currentStation.PawnDestroyedEvent += onPawnDestroyed;

					retval = true;
				}
				else
				{
					retval = false;
				}
			}
			else
			{
				retval = false;
			}
		}
		return retval;
	}

	private bool ShouldSpawn()
	{
		if(playerState == PlayerStateEnum.UNATTACHED &&
		   !IsAttachedToPawn() &&
		   !IsQueuedToSpawn())
			return true;

		return false;
	}

	private bool IsAttachedToPawn()
	{
		foreach( PawnController pawn in gameInfo.GetPawns())
		{
			if( pawn.playerInfo == this)
				return true;
		}

		return false;
	}

	private bool IsQueuedToSpawn()
	{
		foreach(Station station in gameInfo.GetStations())
		{
			if(station.spawnInfo != null)
			{
				if(station.spawnInfo.PlayerInfo == this)
					return true;
			}
		}
		return false;
	}

	private void Detach()
	{
		if(playerState == PlayerStateEnum.UNATTACHED)
			return;

		currentStation = null;

		playerState = PlayerStateEnum.UNATTACHED;
	}

	private Station GetNextPlayerStationSpawn()
	{
		if(AvailableSpawns == null || AvailableSpawns.Count <= 0)
			AvailableSpawns.AddRange(gameInfo.GetStations());
		
		int index = Random.Range(0,AvailableSpawns.Count);
		
		Station station = AvailableSpawns[index] as Station;
		AvailableSpawns.RemoveAt(index);
		
		return station;
	}

	#region event listeners

	void onPawnLifeTimeOver(Station station, PawnController pawn)
	{
		if(station == currentStation)
			Detach();

		station.PawnLifeTimeOverEvent -= onPawnLifeTimeOver;
		station.PawnDestroyedEvent -= onPawnDestroyed;

	}
	void onPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy)
	{
		if(station == currentStation)
			Detach();

		station.PawnLifeTimeOverEvent -= onPawnLifeTimeOver;
		station.PawnDestroyedEvent -= onPawnDestroyed;
	}

	#endregion


}
