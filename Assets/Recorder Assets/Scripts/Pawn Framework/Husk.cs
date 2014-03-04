using UnityEngine;
using System.Collections;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public class Husk : PawnType {

		public override PawnTypeEnum pawnTypeEnum
		{
			get{ return PawnTypeEnum.HUSK;}
		}
		
		public override bool isRecordedPawn()
		{
			return true;
		}
		
		public override Vector3 GetDirectionVector(int index)
		{
			return Vector3.zero;
		}
		
		public override Vector3 GetAimDirection(int index)
		{
			return Vector3.zero;
		}
		
		public override bool GetJump(int index)
		{
			return false;
		}
		
		public override bool GetFire(int index)
		{
			return false;
		}
	}
}
