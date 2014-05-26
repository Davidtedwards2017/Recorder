using UnityEngine;
using System.Collections;
using PawnFramework;
using Effects;
using StageFramework;

public abstract class GameMode: MonoBehaviour 
{
    protected GameObject Core
    {
        get{ return GameObject.FindGameObjectWithTag("GameInfo"); }
    }
    public Score Score
    {
        get{ return Core.GetComponent<Score>(); }
    }

	protected ArrayList AvailableSpawns = new ArrayList();
	protected ArrayList AlivePawns;

	public int StartingLifes;
	public float MaxLifeTime;
	public float BaseLifeTime;
	public float RoundTimeLimit;
	public bool UseTeamSpawns;
    public bool UseLifeTime;

	public float StartRoundCountDownSequenceTime = 2;

	public bool FriendlyFire;

	public int CurrentRound;
	public int NumberOfRounds;

	public int NumberOfPlayers;

	public float playerRespawnTime = 1;
	public float mimicRespawnTime;

	public PlayerInfo[] players;

	public aRoundEndCondition RoundEndCondition;

    public GameObject EndScreen;
   
    public void InitiateGameMode()
    {
        InitateGameStateManagerEventListeners();
        InitiateStationEventListeners();
        InitiateEndRoundCondtion();
        
        SetupLayerCollisions();
        
        InitiatePlayers();
        InitiateScore();
    }

	private void SetupLayerCollisions()
	{
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

	private void InitateGameStateManagerEventListeners()
	{
		GameStateManager.Instance.GameStartEvent += onGameStart;
		GameStateManager.Instance.GameEndEvent += onGameEnd;
		GameStateManager.Instance.RoundStartEvent += onRoundStart;
		GameStateManager.Instance.StartPlayingEvent += onStartPlaying;
		GameStateManager.Instance.RoundEndEvent += onRoundEnd;
	}

	private void InitiateStationEventListeners()
	{
		foreach( Station station in GameInfo.Instance.GetStations())
		{
			station.PawnDestroyedEvent += onStationPawnDestroyed;
			station.PawnLifeTimeOverEvent += onStationPawnLifeTimeOver;
		}
	}

	private void InitiateEndRoundCondtion()
	{
        RoundEndCondition = Core.GetComponent<aRoundEndCondition>();
		if(RoundEndCondition == null)
		{
			Debug.LogWarning("["+GetType()+"] No RoundEndCondition could be found");
			return;
		}

		RoundEndCondition.RoundEndConditionMetEvent += onRoundEndConditionMet;
	}

	private void InitiateScore()
	{
		Score.InitializeScore(NumberOfPlayers);
	}

	protected virtual void InitiatePlayers()
    {
        players = new PlayerInfo[NumberOfPlayers];
    }

	protected abstract void InitiateStations();

	public abstract void PawnSpawned(PawnController pawn);

    public virtual int GetTeamFromPawnType(PawnTypeEnum pawnType)
    {
        switch (pawnType)
        {
            case PawnTypeEnum.DRIVER: return 1;
            case PawnTypeEnum.MIMIC: return 2;
            case PawnTypeEnum.HUSK:
            default: return 0;
        }
    }

	#region events

	protected virtual void onGameStart()
	{
		CurrentRound = 0;
		GameStateManager.Instance.ReadyToStartRound();
	}
	protected virtual void onGameEnd()
	{
		GameStateManager.Instance.ReadyToStartGame();
	}
	protected virtual void onRoundStart()
	{
        WipePawns();
        WipeStations();

		CurrentRound++;
		
		//Start round countdown sequence
		GameObject CountDownSequence = Instantiate(Resources.Load("CountDownSequence")) as GameObject;
		CountDownSequence CountDownSequenceScript = CountDownSequence.GetComponent<CountDownSequence>();
		
		CountDownSequenceScript.CountDownSequenceCompletedEvent += onRoundStartCountDownEnd;
		CountDownSequenceScript.StartCountdown(StartRoundCountDownSequenceTime);

		InitiateStations();
		Score.ResetScores();

        foreach(PlayerInfo player in players)
            player.gameObject.MoveTo(GameInfo.Instance.SpawnQueue);

	}
	protected virtual void onRoundStartCountDownEnd()
	{
		GameStateManager.Instance.ReadyToStartPlaying();
	}
	protected virtual void onStartPlaying(GameState prevState)
	{
		foreach( PawnController pawnCtrl in GameInfo.Instance.GetPawns())
		{
			if(pawnCtrl != null)
				pawnCtrl.SetPlayable(true);
		}
	}
	protected virtual void onRoundEnd()
	{
		WipePawns();
		WipeStations();

        Instantiate(Resources.Load("End Screen"));


		//GameStateManager.Instance.ReadyToStartRound();
	}

	protected virtual void onPawnDeath(PawnController pawn, PawnController killedBy)
    {
        pawn.PawnDeathEvent -= onPawnDeath;
        
        if(pawn.GetPawnType() == PawnTypeEnum.DRIVER)
        {
            PlayerInfo player = pawn.gameObject.GetComponentInChildren<PlayerInfo>();
            PlayerKilled(player, killedBy);
        }
    }

    protected abstract void PlayerKilled(PlayerInfo pawn, PawnController killedBy);

	protected abstract void onStationPawnDestroyed(Station station, PawnController pawn, PawnController destroyedBy);
	protected abstract void onStationPawnLifeTimeOver(Station station, PawnController pawn);

	protected void onRoundEndConditionMet(aRoundEndCondition condition)
	{
		Debug.Log("["+GetType()+"] Round End Condition Met");

		GameStateManager.Instance.ReadyToEndRound();
	}


	#endregion

    #region General functions
	public void RestartGame()
	{
		GameStateManager.Instance.ReadyToStartGame();
	}

	private void WipePawns()
	{
		// Clear all pawns
		foreach(PawnController pawn in GameInfo.Instance.GetPawns())
		{
			Destroy(pawn.gameObject);
		}
	}

	private void WipeStations()
	{
		//Clear out station recordings
		foreach(Station station in GameInfo.Instance.GetStations())
		{
			station.StopSpawning();
			station.StoredRecording = null;
		}
	}

    #endregion

}

