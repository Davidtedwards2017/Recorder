using UnityEngine;
using System.Collections;
using PawnFramework;

namespace StageFramework
{

	public class GM_Versus : GameMode 
	{
        void Awake()
        {
            EndRoundSummaryResourceName = "EndScoreDisplay_Versus";
        }

		protected override void InitiatePlayers()
		{
            base.InitiatePlayers();

            players[0] = PlayerInfo.CreatePlayer(1,1);
            players[1] = PlayerInfo.CreatePlayer(2,2);
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
                station.CanSpawnHusks = false;
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
	
		protected override void PlayerKilled(PlayerInfo player, PawnController killedBy)
		{
		}
		
		protected override void onStationPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy)
		{
	
		}
		protected override void onStationPawnLifeTimeOver(Station station, PawnController pawn)
		{
			Destroy(pawn.gameObject);
		}
		
		#endregion
	}
}