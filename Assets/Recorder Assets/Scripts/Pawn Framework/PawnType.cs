using UnityEngine;
using System.Collections;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public abstract class PawnType : MonoBehaviour {

		public abstract PawnTypeEnum pawnTypeEnum{ get;}
		public PawnController PawnCtrl;
		// Use this for initialization
		void Awake () {
			PawnCtrl = GetComponent<PawnController>();
		}
		
		// Update is called once per frame
		void Update () {
		
		}


		public abstract bool isRecordedPawn();
		public abstract Vector3 GetDirectionVector(int index);
		public abstract Vector3 GetAimDirection(int index);
		public abstract bool GetJump(int index);
		public abstract bool GetFire(int index);
	}	
}
