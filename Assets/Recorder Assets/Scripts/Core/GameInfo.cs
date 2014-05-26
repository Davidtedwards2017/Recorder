using UnityEngine;
using System.Collections;
using System.Linq;
using PawnFramework;

namespace StageFramework
{
	[RequireComponent(typeof(GameStateManager))]
	public class GameInfo : MonoBehaviour {

		private static GameInfo m_Instance;

		public const string TEAM_0_PAWN_LAYERMASK = "Team0_PawnLayerMask";
		public const string TEAM_1_PAWN_LAYERMASK = "Team1_PawnLayerMask";
		public const string TEAM_2_PAWN_LAYERMASK = "Team2_PawnLayerMask";

		public const string TEAM_0_PROJ_LAYERMASK = "Team0_ProjLayerMask";
		public const string TEAM_1_PROJ_LAYERMASK = "Team1_ProjLayerMask";
		public const string TEAM_2_PROJ_LAYERMASK = "Team2_ProjLayerMask";

		public GameMode CurrentGameMode;
        public GameObject SpawnQueue;
               	
		public static GameInfo Instance{ get { return m_Instance; } }

		// Use this for initialization

		void Awake()
		{
            GameObject go = GameObject.FindGameObjectWithTag("GameMode");
			CurrentGameMode = go.GetComponent<GameMode>();
            SpawnQueue = GameObject.Find("PlayerQueue");
			m_Instance = this;
		}

		void Start () {
            //Screen.SetResolution(512, 512, false);
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}

		public Station[] GetStations()
		{
			GameObject[] objects = GameObject.FindGameObjectsWithTag("Station");
			Station[] stations = new Station[objects.Length];

			for( int i = 0; i < objects.Length; i++)
				stations[i] = objects[i].GetComponent<Station>();

			return stations;
		}

		public Station[] GetStations(int teamNumber)
		{
			var teamStations = 
				from n in GetStations()
				where n.TeamNumber == teamNumber
				select n;

			Debug.Log("station count "+teamStations.Count<Station>());

			return teamStations.ToArray();
		}

		public PawnController[] GetPawns()
		{
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Pawn");
			PawnController[] PawnControllers = new PawnController[gameObjects.Length];
			
			for( int i = 0; i < gameObjects.Length; i++)
				PawnControllers[i] = gameObjects[i].GetComponent<PawnController>();
			
			return PawnControllers;
		}

        public PlayerInfo[] GetPlayers()
        {
            return CurrentGameMode.players;
        }
		/*
		public PlayerInfo[] GetPlayerInfo()
		{
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("TimeBomb");
			TimeBomb[] TimeBombs = new TimeBomb[gameObjects.Length];

			for( int i = 0; i < gameObjects.Length; i++)
				TimeBombs[i] = gameObjects[i].GetComponent<TimeBomb>();

			return TimeBombs;
		}
*/


		public static int GetTeamProjLayerMask(int team)
		{
			switch (team)
			{
			case 0:
				return LayerMask.NameToLayer(TEAM_0_PROJ_LAYERMASK);
			case 1:
				return LayerMask.NameToLayer(TEAM_1_PROJ_LAYERMASK);
			case 2:
				return LayerMask.NameToLayer(TEAM_2_PROJ_LAYERMASK);
			default:
				return 0;	
			}
		}

		public static int GetTeamPawnLayerMask(int team)
		{
			switch (team)
			{
			case 0:
				return LayerMask.NameToLayer(TEAM_0_PAWN_LAYERMASK);
			case 1:
				return LayerMask.NameToLayer(TEAM_1_PAWN_LAYERMASK);
			case 2:
				return LayerMask.NameToLayer(TEAM_2_PAWN_LAYERMASK);
			default:
				return 0;	
			}
		}

     	public static Color GetTeamColor(int team)
		{
			switch (team)
			{
			case 0:
				return Color.grey;
			case 1:
				return Color.red;
			case 2:
				return Color.blue;
			default:
				return Color.grey;
			}
		}

		public static int GetTeamCount(int team)
		{
			int count = 0;

			foreach( GameObject g in GameObject.FindGameObjectsWithTag("Pawn"))
			{
				if(g.GetComponent<PawnController>().teamInfo.TeamNumber == team)
					count++;
			}

			return count;
		}


	}
}
