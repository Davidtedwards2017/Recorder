using UnityEngine;
using System.Collections;
using PawnFramework;
using Effects;

namespace StageFramework
{
	public class GM_Skirmish : GameMode 
	{
		/*
		public GM_Skirmish()
		{
			MaxLifeTime = 10;
			BaseLifeTime = 10;
			FriendlyFire = false;
			NumberOfRounds = -1;
			NumberOfTeams = 2;
		}
*/
		protected override void InitiateStationSpawning()
		{
			//get Station to spawn player at this upcoming round
			Station nextSpawnStation = GetNextStationSpawn();
			
			//get every Station that is not being used by a player this upcoming round;
			ArrayList spawnStations = new ArrayList();
			spawnStations.AddRange(gameInfo.GetStations());
			spawnStations.Remove(nextSpawnStation);
			
			AlivePawns = new ArrayList();

			//spawns with recorded pawns
			foreach(Station station in spawnStations)
			{
				if(station.StoredRecording == null)
				{
					//spawn timebomb
					TimeBomb bomb = station.SpawnTimeBomb(MaxLifeTime);
					bomb.DetonationEvent += onTimeBombDetonation;
				}
				else
				{
					//spawn recorded pawn
					PawnController recordedPawn = station.SpawnPawn(player2,PawnTypeEnum.MIMIC, BaseLifeTime);
					recordedPawn.SetTeam(2);
					recordedPawn.PawnDeathEvent += onPawnDeath;
					AlivePawns.Add(recordedPawn);
				}
			}
			
			//spawn controlled pawn
			PawnController controlledPawn = nextSpawnStation.SpawnPawn(player1, PawnTypeEnum.DRIVER, BaseLifeTime);
			controlledPawn.EndLifeTime = MaxLifeTime;
			controlledPawn.SetTeam(1);
			controlledPawn.PawnDeathEvent += onPawnDeath;
			AlivePawns.Add(controlledPawn);
		}
		protected override void InitiatePlayers()
		{
		}
		public override void PawnSpawned(PawnController pawn)
		{
		}
		#region events

		protected override void onStartPlaying(GameState prevState)
		{
			//TODO: implement prevState check
			foreach( TimeBomb bomb in gameInfo.GetTimeBombs())
				bomb.SetActive(true);
			
			//GameObject pawnGameObjects = Ga
			foreach( PawnController pawnCtrl in gameInfo.GetPawns())
			{
				if(pawnCtrl != null)
					pawnCtrl.SetPlayable(true);
			}
			
		}
		
		protected override void onRoundEnd()
		{
			//ensure no timebombs remain
			foreach( TimeBomb bomb in gameInfo.GetTimeBombs())
				Destroy(bomb.gameObject);
			
			foreach( PawnController pawn in gameInfo.GetPawns())
				Destroy(pawn.gameObject);
			
			foreach( GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile"))
				Destroy(projectile);
			
			//figure out who one winner
			if( NumberOfRounds > 0 && CurrentRound > NumberOfRounds)
			{
				gameInfo.GSM.ReadyToEndGame();
			}
			else 
			{
				gameInfo.GSM.ReadyToStartRound();
			}
		}
		
		protected override void onPawnDeath(PawnController pawn, PawnController killedBy)
		{
			pawn.PawnDeathEvent -= onPawnDeath;
			
			AlivePawns.Remove(pawn);
			
			if(gameInfo.GetTimeBombs().Length > 0)
				return;
			
			if(AlivePawns.Count == 0)
				gameInfo.GSM.ReadyToEndRound();

			
			if(gameInfo.GetPawns().Length <= 1)
				gameInfo.GSM.ReadyToEndRound();
		}
		
		protected override void onTimeBombDetonation()
		{
			foreach( TimeBomb bomb in gameInfo.GetTimeBombs())
				bomb.DetonationEvent -= onTimeBombDetonation;
			
			gameInfo.GSM.ReadyToEndRound();
		}
		protected override void PlayerKilled(PawnController pawn, PawnController killedBy)
		{
		}

		protected override void onStationPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy)
		{
		}
		protected override void onStationPawnLifeTimeOver(Station station, PawnController pawn)
		{
		}

		#endregion

	}
}
