using UnityEngine;
using System.Collections;


namespace StageFramework
{
	public class GM_Skirmish : GameMode 
	{

		public GM_Skirmish()
		{
			MaxLifeTime = 10;
			BaseLifeTime = 10;
			FriendlyFire = false;
			NumberOfRounds = -1;
			NumberOfTeams = 2;
		}

		public override bool GameEndCondition()
		{
			return false;
		}

		public override bool RoundEndCondition()
		{
			//check if only pawn is left standing
			return false;
		}

	}
}
