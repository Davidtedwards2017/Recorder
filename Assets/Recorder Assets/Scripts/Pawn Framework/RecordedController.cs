using UnityEngine;
using System.Collections;

namespace PawnFramework
{

	public class RecordedController : PawnController {

		public int TimeIndex = 0;
		//public delegate void RecordEndEventHandler();
		//public event RecordEndEventHandler RecordEndEvent;

		//void onRecordEnd() { if( RecordEndEvent != null) RecordEndEvent(); }
		void Start()
		{
			LifeTime = Recording.Length * Time.deltaTime;
		}
		// Update is called once per frame
		void LateUpdate () 
		{
		
			if(bPlaying)
			{
				TimeIndex++;
				LifeTime -= Time.deltaTime;
				if(TimeIndex >= Recording.Length)
				{
					bPlaying = false;
					RecordingEnded();
				}
			}

		}

		public void RecordingEnded()
		{
			DespawnPawn();
		}

		public override bool isRecordedPawn()
		{
			return true;
		}

		public void SetRecord(Record record)
		{
			Recording = record;
		}

		public override Vector3 GetDirectionVector()
		{
			return Recording.GetRecordData(TimeIndex).DirectionVector;
		}

		public override Vector3 GetAimDirection()
		{
			return Recording.GetRecordData(TimeIndex).AimDirection;
		}

		public override bool GetJump()
		{
			return Recording.GetRecordData(TimeIndex).Jump;
		}

		public override bool GetFire()
		{
			return Recording.GetRecordData(TimeIndex).Fire;
		}
	}
}
