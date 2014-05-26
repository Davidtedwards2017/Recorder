using UnityEngine;
using System.Collections;

using PawnFramework;
using Effects;

namespace StageFramework
{

	public class GM_Husk : GameMode {

		protected override void InitiatePlayers()
		{
            base.InitiatePlayers();

            players[0] = PlayerInfo.CreatePlayer(1,1);
		}

		protected PlayerInfo InstantiatePlayer(int player, int team)
		{
			PlayerInfo playerInfo = gameObject.AddComponent<PlayerInfo>();
			playerInfo.InstantiatePlayerInfo(player, team);
			return playerInfo;
		}

		protected override void InitiateStations()
		{
			foreach(Station station in GameInfo.Instance.GetStations())
			{
                station.CanSpawnHusks = true;
				station.QueueSpawn();
                station.AutoSpawn = true;
			}
		}

		public override void PawnSpawned(PawnController pawn)
		{
			pawn.PawnDeathEvent += onPawnDeath;
			
			if(GameStateManager.Instance.CurrentGameState == GameState.Playing)
			{
				pawn.SetPlayable(true);
			}
		}

     	#region events
						
		protected override void PlayerKilled(PlayerInfo pawn, PawnController killedBy)
		{

		}

		protected override void onStationPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy)
		{
           
		}
		protected override void onStationPawnLifeTimeOver(Station station, PawnController pawn)
		{
            if(pawn.GetPawnType() != PawnTypeEnum.DRIVER)
            {
                pawn.InitiateForPawnType<Husk>();
            }
            else
            {
                Destroy(pawn.gameObject);
            }
                //ChangePawnType(PawnTypeEnum.HUSK);
        }

		#endregion
	}
}
