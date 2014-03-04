using UnityEngine;
using System.Collections;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public class Driver : PawnType {

		public override PawnTypeEnum pawnTypeEnum
		{
			get{ return PawnTypeEnum.DRIVER;}
		}

		public override bool isRecordedPawn()
		{
			return false;
		}

		public override Vector3 GetDirectionVector(int index)
		{
			if(!PawnCtrl.bPlaying)
				return Vector3.zero;

			Vector3 dirVec = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			//GetRecordData(index).DirectionVector = dirVec;

			return dirVec;
		}
		public override Vector3 GetAimDirection(int index)
		{
			if(!PawnCtrl.bPlaying)
				return Vector3.zero;

			Vector3 vec = Input.mousePosition;
			vec.z = Mathf.Abs(Camera.main.transform.position.z);
			vec = Camera.main.ScreenToWorldPoint(vec);

			vec = (vec - transform.position).normalized;

			GetRecordData(index).AimDirection = vec;

			return vec;
		}
		public override bool GetJump(int index)
		{
			if(!PawnCtrl.bPlaying)
				return false;

			bool jump = Input.GetButton("Jump");

			GetRecordData(index).Jump = jump;

			return jump;
		}
		public override bool GetFire(int index)
		{
			if(!PawnCtrl.bPlaying)
				return false;

			bool fire = Input.GetButton("Fire1");
			GetRecordData(index).Fire = fire;
			return fire;
		}

		RecordData GetRecordData(int index)
		{
			if(PawnCtrl.Recording.GetRecordData(index) == null)
				PawnCtrl.Recording.AddRecordData();

			return PawnCtrl.Recording.GetRecordData(index);
		}

	}
}