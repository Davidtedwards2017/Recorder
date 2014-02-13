using UnityEngine;
using System.Collections;

namespace PawnFramework
{

	public class PlayerController : PawnController {

		bool bRecording = false;
		void Start()
		{
			Recording = new Record();
		}

		// Update is called once per frame
		void LateUpdate () {
			if(bPlaying && bRecording)
			{
				Recording.AddRecordData(GetDirectionVector(), 
				                          GetAimDirection(),
				                          GetJump(),
				                          GetFire()
				                          );
			}

			if(bPlaying)
			{
				LifeTime -= Time.deltaTime;

				if(LifeTime <= 0)
					DespawnPawn();
			}

		}

		public override void SetPlayable(bool value)
		{
			motor.SetControllable(value);
			bPlaying = value;
			bRecording = value;
		}

		public override bool isRecordedPawn()
		{
			return false;
		}
		public override Vector3 GetDirectionVector()
		{
			if(!bPlaying)
				return Vector3.zero;

			Vector3 dirVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

			return dirVec;
		}
		public override Vector3 GetAimDirection()
		{
			if(!bPlaying)
				return Vector3.zero;

			Vector3 vec = Input.mousePosition;
			vec.z = Mathf.Abs(Camera.main.transform.position.z);
			vec = Camera.main.ScreenToWorldPoint(vec);
			
			return (vec - transform.position).normalized;
		}
		public override bool GetJump()
		{
			if(!bPlaying)
				return false;

			return Input.GetButton("Jump");
		}
		public override bool GetFire()
		{
			if(!bPlaying)
				return false;

			return Input.GetButton("Fire1");
		}

	}
}