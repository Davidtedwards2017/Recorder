using UnityEngine;
using System.Collections;
using PawnFramework;

namespace StageFramework
{
	public enum GameState
	{
		Uninitialized, Loading, Loaded, GameStart, RoundStart, Playing, Paused, RoundEnd, GameEnd
	}

	public class GameStateManager : MonoBehaviour 
	{
		private static GameStateManager m_Instance;
		private GameState m_CurrentGameState;

		public static GameStateManager Instance
		{
			get { return m_Instance; }
		}

		public GameState CurrentGameState
		{
			get	{ return m_CurrentGameState; }
			set
			{
				GameState oldState = m_CurrentGameState;
				m_CurrentGameState = value;
				GameStateChanged(m_CurrentGameState, oldState);
			}
		}

		#region GameState Transition Flags
		private bool bFinishLoading = true;
		private bool bStartGame = true;
		private bool bStartRound = false;
		private bool bStartPlaying = false;
		private bool bPause = false;
		private bool bEndRound = false;
		private bool bEndGame = false;
		private bool bReturnToStart = false;
		#endregion

		#region Events declarations
		public delegate void LoadingEventHandler();
		public delegate void LoadedEventHander();

		public delegate void StartPlayingEventHandler(GameState prevState);

		public delegate void PausedEventHandler(GameState prevState);
		public delegate void UnpausedEventHandler();

		public delegate void RoundStartEventHandler();
		public delegate void RoundEndEventHandler();

		public delegate void GameStartEventHandler();
		public delegate void GameEndEventHandler();

		public event LoadingEventHandler LoadingEvent;
		public event LoadedEventHander LoadedEvent;

		public event RoundStartEventHandler RoundStartEvent;
		public event RoundEndEventHandler RoundEndEvent;

		public event StartPlayingEventHandler StartPlayingEvent;

		public event PausedEventHandler PausedEvent;
		public event UnpausedEventHandler UnpausedEvent;

		public event GameStartEventHandler GameStartEvent;
		public event GameEndEventHandler GameEndEvent;

		void onLoading() { if( LoadingEvent != null) LoadedEvent(); }
		void onLoaded() { if( LoadedEvent != null) LoadedEvent();}
		void onRoundStart() { if( RoundStartEvent != null) RoundStartEvent(); } 
		void onRoundEnd() 
		{ 
			if( RoundEndEvent != null) 
				RoundEndEvent();
		}
		void onGameStart() 
		{ 
			if( GameStartEvent != null) 
				GameStartEvent(); 
		} 
		void onGameEnd() 
		{ 
           	//TODO: Decide winner

			//TODO: Display EndGame Screen

			//TODO: Wait for user input

			if( GameEndEvent != null) 
				GameEndEvent();	
		}
		void onStartPlaying(GameState prevState) 
		{ 
            Time.timeScale = 1;

			if( StartPlayingEvent != null) 
				StartPlayingEvent(prevState); 
		}
		void onPaused(GameState prevState) 
		{ 
			Time.timeScale = 0;

			if( PausedEvent != null) 
				PausedEvent(prevState); 
		}
		void onUnpaused()
		{
			Time.timeScale = 1;

			if(UnpausedEvent != null)
				UnpausedEvent();
		}
		#endregion

		// Use this for initialization
		void Awake () 
		{
			m_Instance = this;
			CurrentGameState = GameState.Uninitialized;
			CurrentGameState = GameState.Loading;
		}
		
		// Update is called once per frame
		void Update () 
		{
			switch (CurrentGameState)
			{
			case GameState.Loading:
				if(ShouldFinishLoading())	CurrentGameState = GameState.Loaded;
				break;
			case GameState.Loaded:
				if(ShouldStartGame()) CurrentGameState = GameState.GameStart;
				break;
			case GameState.GameStart:
				if(ShouldStartRound()) CurrentGameState = GameState.RoundStart;
				break;
			case GameState.RoundStart:
				if(ShouldStartPlaying()) CurrentGameState = GameState.Playing;
				break;
			case GameState.Paused:
				if(ShouldStartPlaying())
				{ 
					CurrentGameState = GameState.Playing;
				}
				else if(ShouldStartGame()) 
				{
					CurrentGameState = GameState.GameStart;
				}
				break;
			case GameState.Playing:
				if(ShouldEndRound()) CurrentGameState = GameState.RoundEnd;
				else if (ShouldPause()) CurrentGameState = GameState.Paused;
				break;
			case GameState.RoundEnd:
				if(ShouldEndGame()) CurrentGameState = GameState.GameEnd;
				else if(ShouldStartRound()) CurrentGameState = GameState.RoundStart;
                if(ShouldStartGame()) CurrentGameState = GameState.GameStart;
				break;
			case GameState.GameEnd:
				if(ShouldStartGame()) CurrentGameState = GameState.GameStart;
                else if(ShouldStartRound()) CurrentGameState = GameState.RoundStart;
				break;
			default:
				break;
			}
		}

		#region GameState Transition methods

		public void ReadyToFinishLoading()
		{
			if(CurrentGameState == GameState.Loading)
				bFinishLoading = true;
		}
		public void ReadyToStartGame()
		{
			if(CurrentGameState == GameState.Loaded ||
			  	CurrentGameState == GameState.Paused ||
                CurrentGameState == GameState.RoundEnd)
				bStartGame = true;
		}
		public void ReadyToStartRound()
		{
			if(CurrentGameState == GameState.GameStart ||
			   CurrentGameState == GameState.RoundEnd )
				bStartRound = true;
		}
		public void ReadyToStartPlaying()
		{
			if(CurrentGameState == GameState.RoundStart ||
			   CurrentGameState == GameState.Paused) 
				bStartPlaying = true;
		}
		public void ReadyToPause()
		{
			if(CurrentGameState == GameState.Playing)
				bPause = true;
		}
		public void ReadyToEndRound()
		{
			if(CurrentGameState == GameState.Playing)
				bEndRound = true;
		}
		public void ReadyToEndGame()
		{
			if(CurrentGameState == GameState.RoundEnd)
				bEndGame = true;
		}
		public void ReadyToReturnToStartScreen()
		{
			//TODO: Implement
			bReturnToStart = true;
		}

		private bool ShouldFinishLoading()
		{
			if(bFinishLoading)
			{
				bFinishLoading = false;
				return true;
			}
			return false;
		}
		private bool ShouldStartGame()
		{
			if(bStartGame)
			{
				bStartGame = false;
				return true;
			}
			return false;
		}
		private bool ShouldStartRound()
		{
			if(bStartRound)
			{
				bStartRound = false;
				return true;
			}
			return false;
		}
		private bool ShouldStartPlaying()
		{
			if(bStartPlaying)
			{
				bStartPlaying = false;
				return true;
			}
			return false;
		}
		private bool ShouldPause()
		{
			if(bPause)
			{
				bPause = false;
				return true;
			}
			return false;
		}
		private bool ShouldEndRound()
		{
			if(bEndRound)
			{
				bEndRound = false;
				return true;
			}
			return false;
		}	
		private bool ShouldEndGame()
		{
			if(bEndGame)
			{
				bFinishLoading = false;
				return true;
			}
			return false;
		}
		private bool ShouldReturnToStartScreen()
		{
			if(bReturnToStart)
			{
				bReturnToStart = false;
				return true;
			}
			return false;
		}

		#endregion

		private void GameStateChanged(GameState newState, GameState oldState)
		{
			if(oldState == GameState.Paused)
				onUnpaused();

			switch(newState)
			{
			case GameState.Loading:
				onLoading();
				break;
			case GameState.Loaded:
				onLoaded();
				break;
			case GameState.RoundStart:
				onRoundStart();
				break;
			case GameState.RoundEnd:
				onRoundEnd();
				break;
			case GameState.GameStart:
				onGameStart();
				break;
			case GameState.GameEnd:
				onGameEnd();
				break;
			case GameState.Playing:
				onStartPlaying(oldState);
				break;
			case GameState.Paused:
				onPaused(oldState);
				break;
			default:
				break;
			}

			Debug.Log("[GameStateChanged] stage changed from " + oldState + " to " +newState);
		}


	}
}
