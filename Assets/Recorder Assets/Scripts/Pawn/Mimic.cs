using UnityEngine;
using System.Collections;
using StageFramework;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public class Mimic : PawnType {

        public int KillAwardAmount = 300;

        public override void InitiatePawnTypeVisuals(int team)
        {
            Material newMat = Resources.Load("MimicMat", typeof(Material)) as Material;
            
            Renderer renderer = GetPawnModel().GetComponent<Renderer>();
            renderer.material = newMat;
            //renderer.material.SetColor("_MainColor", GameInfo.GetTeamColor(team));
        }

		public override PawnTypeEnum pawnTypeEnum
		{
			get{ return PawnTypeEnum.MIMIC;}
		}
	
        public override int GetAwardedKillAmount()
        {
            return KillAwardAmount;
        }

		public override Vector3 GetDirectionVector(int index)
		{
			if( Pawn.Recording.GetRecordData(index) == null)
			{
				Debug.Log(string.Format("{0}, RecordData at index {1} is null", gameObject.name, index));
			}

			return Pawn.Recording.GetRecordData(index).DirectionVector;
		}

       	public override Vector3 GetAimDirection(int index)
		{
			return Pawn.Recording.GetRecordData(index).AimDirection;
		}

		public override bool GetJump(int index)
		{
			return Pawn.Recording.GetRecordData(index).Jump;
		}

		public override bool GetFire(int index)
		{
			return Pawn.Recording.GetRecordData(index).Fire;
		}
	}
}
