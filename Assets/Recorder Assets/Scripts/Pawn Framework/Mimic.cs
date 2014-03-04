using UnityEngine;
using System.Collections;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public class Mimic : PawnType {

		public override PawnTypeEnum pawnTypeEnum
		{
			get{ return PawnTypeEnum.MIMIC;}
		}
	
		public override bool isRecordedPawn()
		{
			return true;
		}

		public override Vector3 GetDirectionVector(int index)
		{
			return PawnCtrl.Recording.GetRecordData(index).DirectionVector;
		}

		public override Vector3 GetAimDirection(int index)
		{
			return PawnCtrl.Recording.GetRecordData(index).AimDirection;
		}

		public override bool GetJump(int index)
		{
			return PawnCtrl.Recording.GetRecordData(index).Jump;
		}

		public override bool GetFire(int index)
		{
			return PawnCtrl.Recording.GetRecordData(index).Fire;
		}
	}
}
