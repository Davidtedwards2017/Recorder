using UnityEngine;
using System.Collections;
using PawnFramework;

namespace StageFramework
{
	[RequireComponent(typeof(GameStateManager))]
	public class GameInfo : MonoBehaviour {

		public const string TEAM_1_PAWN_LAYERMASK = "Team1_PawnLayerMask";
		public const string TEAM_2_PAWN_LAYERMASK = "Team2_PawnLayerMask";
		public const string TEAM_1_PROJ_LAYERMASK = "Team1_ProjLayerMask";
		public const string TEAM_2_PROJ_LAYERMASK = "Team2_ProjLayerMask";

		public GameMode CurrentGameMode;
		public GameStateManager GSM;

		// Use this for initialization

		void Awake()
		{
			GSM = GetComponent<GameStateManager>();
		}

		void Start () {

		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}


		public GameObject[] GetStations()
		{
			return GameObject.FindGameObjectsWithTag("Station");
		}

		public PawnController[] GetPawns()
		{
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Pawn");
			PawnController[] PawnControllers = new PawnController[gameObjects.Length];
			
			for( int i = 0; i < gameObjects.Length; i++)
				PawnControllers[i] = gameObjects[i].GetComponent<PawnController>();
			
			return PawnControllers;
		}

		public TimeBomb[] GetTimeBombs()
		{
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("TimeBomb");
			TimeBomb[] TimeBombs = new TimeBomb[gameObjects.Length];

			for( int i = 0; i < gameObjects.Length; i++)
				TimeBombs[i] = gameObjects[i].GetComponent<TimeBomb>();

			return TimeBombs;
		}

		public static int GetTeamProjLayerMask(int team)
		{
			switch (team)
			{
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
			case 1:
				return Color.red;
			case 2:
				return Color.blue;
			default:
				return Color.gray;
			}
		}

		public static int GetTeamCount(int team)
		{
			int count = 0;

			foreach( GameObject g in GameObject.FindGameObjectsWithTag("Pawn"))
			{
				if(g.GetComponent<PawnController>().team == team)
					count++;
			}

			return count;
		}
	}
}
