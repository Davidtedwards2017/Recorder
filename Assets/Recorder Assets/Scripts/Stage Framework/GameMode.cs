using UnityEngine;
using System.Collections;
using PawnFramework;
using Effects;

namespace StageFramework
{
	public abstract class GameMode: MonoBehaviour 
	{
		protected ArrayList AvailableSpawns = new ArrayList();
		protected ArrayList AlivePawns;

		protected GameInfo gameInfo;

		public int StartingLifes;
		public float MaxLifeTime;
		public float BaseLifeTime;
		public float RoundTimeLimit;

		public float TimeBombLifeTime;

		public float StartRoundCountDownSequenceTime;

		public bool FriendlyFire;

		public int CurrentRound;
		public int NumberOfRounds;

		public int NumberOfTeams;
		public int NumberOfPlayers;

		public PlayerInfo player1;
		public PlayerInfo player2;
		public PlayerInfo player0;

		public PlayerInfo[] players;

		void Awake()
		{
			this.gameInfo = GetComponent<GameInfo>();

			//Pawns Ignore Collisions between eachother
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(0),GameInfo.GetTeamPawnLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(0),GameInfo.GetTeamPawnLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(0),GameInfo.GetTeamPawnLayerMask(2), true);

			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(1),GameInfo.GetTeamPawnLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(1),GameInfo.GetTeamPawnLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(1),GameInfo.GetTeamPawnLayerMask(2), true);

			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(2),GameInfo.GetTeamPawnLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(2),GameInfo.GetTeamPawnLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(2),GameInfo.GetTeamPawnLayerMask(2), true);

			//Projectiles Ignore Collisions between eachother
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(0),GameInfo.GetTeamProjLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(0),GameInfo.GetTeamProjLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(0),GameInfo.GetTeamProjLayerMask(2), true);

			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamProjLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamProjLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamProjLayerMask(2), true);

			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamProjLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamProjLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamProjLayerMask(2), true);

			//Projectiles Ignore Collisions of team who spawned them
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(0),GameInfo.GetTeamPawnLayerMask(0), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamPawnLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamPawnLayerMask(2), true);
		}

		void Start()
		{
			InitateGameStateManagerEventListeners();
			InitiateStationEventListeners();
			InitiatePlayers();
		}

		private void InitateGameStateManagerEventListeners()
		{
			this.gameInfo.GSM.GameStartEvent += onGameStart;
			this.gameInfo.GSM.GameEndEvent += onGameEnd;
			this.gameInfo.GSM.RoundStartEvent += onRoundStart;
			this.gameInfo.GSM.StartPlayingEvent += onStartPlaying;
			this.gameInfo.GSM.RoundEndEvent += onRoundEnd;
		}

		private void InitiateStationEventListeners()
		{
			foreach( Station station in gameInfo.GetStations())
			{
				station.PawnDestroyedEvent += onStationPawnDestroyed;
				station.PawnLifeTimeOverEvent += onStationPawnLifeTimeOver;
			}
		}

		protected abstract void InitiatePlayers();

		protected Station GetNextStationSpawn()
		{
			if(AvailableSpawns == null || AvailableSpawns.Count <= 0)
				AvailableSpawns.AddRange(gameInfo.GetStations());

			int index = Random.Range(0,AvailableSpawns.Count);

			Station station = AvailableSpawns[index] as Station;
			AvailableSpawns.RemoveAt(index);

			return station;
		}

		protected abstract void InitiateStationSpawning();

		public abstract void PawnSpawned(PawnController pawn);

		#region events

		protected virtual void onGameStart()
		{
			CurrentRound = 0;
			gameInfo.GSM.ReadyToStartRound();
		}
		protected virtual void onGameEnd()
		{
			gameInfo.GSM.ReadyToStartGame();
		}

		protected virtual void onRoundStart()
		{
			CurrentRound++;
			
			//Start round countdown sequence
			GameObject CountDownSequence = Instantiate(Resources.Load("CountDownSequence")) as GameObject;
			CountDownSequence CountDownSequenceScript = CountDownSequence.GetComponent<CountDownSequence>();
			
			CountDownSequenceScript.CountDownSequenceCompletedEvent += onRoundStartCountDownEnd;
			CountDownSequenceScript.StartCountdown(StartRoundCountDownSequenceTime);
			
			InitiateStationSpawning();
		}
		
		protected virtual void onRoundStartCountDownEnd()
		{
			gameInfo.GSM.ReadyToStartPlaying();
		}
		protected abstract void onStartPlaying(GameState prevState);
		protected abstract void onRoundEnd();
		protected abstract void onPawnDeath(PawnController pawn, PawnController killedBy);
		protected abstract void onTimeBombDetonation();

		protected abstract void PlayerKilled(PawnController pawn, PawnController killedBy);

		protected abstract void onStationPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy);
		protected abstract void onStationPawnLifeTimeOver(Station station, PawnController pawn);

		#endregion



	}
}
