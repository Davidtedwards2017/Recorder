using UnityEngine;
using System.Collections;

using PawnFramework;
using Effects;

namespace StageFramework
{

	public class GM_Husk : GameMode {

		protected override void InitiateStationSpawning()
		{
			//QueueDriverSpawn(player1);
		}

		protected override void InitiatePlayers()
		{
			player1 = new PlayerInfo(1,1, gameInfo);
			player2 = new PlayerInfo(2,2, gameInfo);
			player0 = new PlayerInfo(0,0, gameInfo);

			players = new PlayerInfo[NumberOfPlayers];
			players[0] = player1;
		}

		void Update()
		{
			foreach( PlayerInfo player in players)
			{
				if(player.playerState == PlayerInfo.PlayerStateEnum.UNATTACHED)
					player.TryToSpawn();
			}
		}

		public override void PawnSpawned(PawnController pawn)
		{
			pawn.PawnDeathEvent += onPawnDeath;
			
			if(gameInfo.GSM.CurrentGameState == GameState.Playing)
			{
				pawn.SetPlayable(true);
			}
		}

		#region events
						
		protected override void onStartPlaying(GameState prevState)
		{
			//GameObject pawnGameObjects = Ga
			foreach( PawnController pawnCtrl in gameInfo.GetPawns())
			{
				if(pawnCtrl != null)
					pawnCtrl.SetPlayable(true);
			}
		}
		
		protected override void onRoundEnd()
		{

		}
		
		protected override void onPawnDeath(PawnController pawn, PawnController killedBy)
		{
			pawn.PawnDeathEvent -= onPawnDeath;

			Station station = pawn.station;

			if(pawn.GetPawnType() == PawnTypeEnum.DRIVER)
			{
				PlayerKilled(pawn, killedBy);
			}

			if(station.StoredRecording != null)
			{
				//spawn mimic
				station.OverrideStationSpawn(player2, PawnTypeEnum.MIMIC);
				//StartStationSpawn(station, player2, PawnTypeEnum.MIMIC, 2);
			}

		}

		protected override void PlayerKilled(PawnController pawn, PawnController killedBy)
		{

		}

		protected override void onTimeBombDetonation()
		{

		}

		protected override void onStationPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy)
		{

		}
		protected override void onStationPawnLifeTimeOver(Station station, PawnController pawn)
		{
			PawnTypeEnum pawnType = pawn.GetPawnType();

			pawn.ChangePawnType(PawnTypeEnum.HUSK);

			if(pawnType == PawnTypeEnum.DRIVER)
			{
				//QueueDriverSpawn(player1);
				//SpawnNewDriver();
			}
			else if(pawnType == PawnTypeEnum.MIMIC)
			{
				SpawnInfo spawnInfo = station.spawnInfo;

				if(spawnInfo != null)
				{
					if(spawnInfo.PawnSpawnType == PawnTypeEnum.DRIVER)
					{
						PawnController pawnCtrl = station.PossessExistingHusk(spawnInfo.PlayerInfo);
						if(pawnCtrl != null || gameInfo.GSM.CurrentGameState == GameState.Playing)
							pawnCtrl.SetPlayable(true);

						Destroy(spawnInfo);
					}
				}
			}
		}

		#endregion
	}
}
