using UnityEngine;
using System.Collections;
using StageFramework;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public abstract class PawnType : MonoBehaviour {

		public abstract PawnTypeEnum pawnTypeEnum{ get;}
        public abstract int GetAwardedKillAmount();
        public PawnController Pawn;

        public float EndLifeTime;
        public float CurrentLifeTime = 0f;
		// Use this for initialization
		protected virtual void Awake () {
			Pawn = GetComponent<PawnController>();
		}
		
        protected GameObject GetPawnModel()
        {
            return transform.FindChild("Pawn Model").gameObject;
        }
        public abstract void InitiatePawnTypeVisuals(int team);
		public abstract Vector3 GetDirectionVector(int index);
		public abstract Vector3 GetAimDirection(int index);
		public abstract bool GetJump(int index);
		public abstract bool GetFire(int index);
	}	
}
