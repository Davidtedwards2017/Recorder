using UnityEngine;
using System.Collections;
using PawnFramework;
using Effects;
using PawnFramework;

namespace StageFramework
{
	public abstract class GameMode: MonoBehaviour 
	{
		protected ArrayList AvailableSpawns = new ArrayList();
		protected ArrayList AlivePawns;

		protected GameInfo GameInfo;

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

		public abstract bool GameEndCondition();
		public abstract bool RoundEndCondition();

		void Awake()
		{
			this.GameInfo = GetComponent<GameInfo>();

			//Pawns Ignore Collisions between eachother
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(1),GameInfo.GetTeamPawnLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(2),GameInfo.GetTeamPawnLayerMask(2), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(1),GameInfo.GetTeamPawnLayerMask(2), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamPawnLayerMask(2),GameInfo.GetTeamPawnLayerMask(1), true);

			//Projectiles Ignore Collisions between eachother
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamProjLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamProjLayerMask(2), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamProjLayerMask(2), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamProjLayerMask(1), true);

			//Projectiles Ignore Collisions of team who spawned them
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(1),GameInfo.GetTeamPawnLayerMask(1), true);
			Physics.IgnoreLayerCollision(GameInfo.GetTeamProjLayerMask(2),GameInfo.GetTeamPawnLayerMask(2), true);
		}

		void Start()
		{
			this.GameInfo.GSM.GameStartEvent += onGameStart;
			this.GameInfo.GSM.GameEndEvent += onGameEnd;
			this.GameInfo.GSM.RoundStartEvent += onRoundStart;
			this.GameInfo.GSM.StartPlayingEvent += onStartPlaying;
			this.GameInfo.GSM.RoundEndEvent += onRoundEnd;
		}

		protected GameObject GetNextStationSpawn()
		{
			if(AvailableSpawns == null || AvailableSpawns.Count <= 0)
				AvailableSpawns.AddRange(GameInfo.GetStations());

			int index = Random.Range(0,AvailableSpawns.Count);

			GameObject station = AvailableSpawns[index] as GameObject;
			AvailableSpawns.RemoveAt(index);

			return station;
		}

		protected virtual void InitiateStationSpawning()
		{
			//get Station to spawn player at this upcoming round
			GameObject nextSpawnStation = GetNextStationSpawn();

			//get every Station that is not being used by a player this upcoming round;
			ArrayList spawnStations = new ArrayList();
			spawnStations.AddRange(GameInfo.GetStations());
			spawnStations.Remove(nextSpawnStation);

			AlivePawns = new ArrayList();

			Station station;

			//spawns with recorded pawns
			foreach(GameObject g in spawnStations)
			{
				station = g.GetComponent<Station>();
				if(station.StoredRecording == null)
				{
					//spawn timebomb
					TimeBomb bomb = station.SpawnTimeBomb(MaxLifeTime);
					bomb.DetonationEvent += onTimeBombDetonation;
				}
				else
				{
					//spawn recorded pawn
					PawnController recordedPawn = station.SpawnPawn(new PawnInfo(0,2));
					recordedPawn.SetTeam(2);
					recordedPawn.PawnDeathEvent += onPawnDeath;
					AlivePawns.Add(recordedPawn);
				}
			}

			//spawn controlled pawn
			station = nextSpawnStation.GetComponent<Station>();
			PawnController controlledPawn = station.SpawnPawn(new PawnInfo(1,1));
			controlledPawn.LifeTime = MaxLifeTime;
			controlledPawn.SetTeam(1);
			controlledPawn.PawnDeathEvent += onPawnDeath;
			AlivePawns.Add(controlledPawn);
		}

		#region events

		protected virtual void onGameStart()
		{
			CurrentRound = 0;
			GameInfo.GSM.ReadyToStartRound();
		}
		
		protected virtual void onGameEnd()
		{
			//TODO: Prompt player(s) to restart or return to title screen
			GameInfo.GSM.ReadyToStartGame();
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
			GameInfo.GSM.ReadyToStartPlaying();
		}

		protected virtual void onStartPlaying(GameState prevState)
		{
			//TODO: implement prevState check
 			foreach( TimeBomb bomb in GameInfo.GetTimeBombs())
 				bomb.SetActive(true);

			//GameObject pawnGameObjects = Ga
			foreach( PawnController pawnCtrl in GameInfo.GetPawns())
			{
				if(pawnCtrl != null)
					pawnCtrl.SetPlayable(true);
			}

		}

		protected virtual void onRoundEnd()
		{
			//ensure no timebombs remain
			foreach( TimeBomb bomb in GameInfo.GetTimeBombs())
				Destroy(bomb.gameObject);

			foreach( PawnController pawn in GameInfo.GetPawns())
				Destroy(pawn.gameObject);

			foreach( GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile"))
				Destroy(projectile);

			//figure out who one winner
			if( NumberOfRounds > 0 && CurrentRound > NumberOfRounds)
			{
				GameInfo.GSM.ReadyToEndGame();
			}
			else 
			{
				GameInfo.GSM.ReadyToStartRound();
			}
		}

		protected virtual void onPawnDeath(PawnController pawn, GameObject killedBy)
		{
			pawn.PawnDeathEvent -= onPawnDeath;

			AlivePawns.Remove(pawn);

			if(GameInfo.GetTimeBombs().Length > 0)
				return;

			if(AlivePawns.Count == 0)
				GameInfo.GSM.ReadyToEndRound();
			/*
			if(GameInfo.GetTeamCount(pawn.team) <= 1)
			{
				GameInfo.GSM.ReadyToEndRound();
			}
			*/

			if(GameInfo.GetPawns().Length <= 1)
				GameInfo.GSM.ReadyToEndRound();
		}

		protected virtual void onTimeBombDetonation()
		{
			foreach( TimeBomb bomb in GameInfo.GetTimeBombs())
				bomb.DetonationEvent -= onTimeBombDetonation;

			GameInfo.GSM.ReadyToEndRound();
		}

		#endregion



	}
}
