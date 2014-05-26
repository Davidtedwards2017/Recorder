using UnityEngine;
using System.Collections;
using StageFramework;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public class Husk : PawnType {

		public HuskGun HuskGunPrefab;
        public int KillAwardAmount = 100;
		public float AimSnapAngle = 45;
		public float MaxAimRange = 100;
		public bool AutoFire = true;

        public GameObject hostileTarget;

		void Start()
		{
			if(Pawn.gun != null)
			{
				Destroy(Pawn.gun.gameObject);

				GameObject go = Instantiate(Resources.Load<GameObject>("HuskGunPrefab"), transform.position, Quaternion.identity) as GameObject;
				go.transform.parent = Pawn.transform;
			}
		}

        void Update()
        {
            hostileTarget = GetClosestHostileLocation();
        }

        GameObject GetClosestHostileLocation()
		{
            GameObject closestHostile = null;
			Vector3 closestLoc = Vector3.forward * 10000; // really far distance

			GameObject[] pawnGameObjects = GameObject.FindGameObjectsWithTag("Pawn");
        

			foreach( GameObject pawnGameObject in pawnGameObjects)
			{
                PawnController ctrl = pawnGameObject.GetComponent<PawnController>();

				if(ctrl == null || ctrl == this || ctrl.GetPawnType() == PawnTypeEnum.HUSK)
					continue;

				if(Vector3.Distance(this.transform.position, ctrl.transform.position) < 
				   Vector3.Distance(this.transform.position, closestLoc))
                {
                    closestHostile = ctrl.gameObject;
					closestLoc = ctrl.transform.position;
                }
			}

            return closestHostile;
		}

		Vector3 GetHuskAutoAimDirection()
		{
            if(hostileTarget == null)
            {
              return Vector3.zero;
            }

            Vector3 aimAtLocation = hostileTarget.transform.position;

			if(Vector3.Distance(this.transform.position, aimAtLocation) > MaxAimRange)
				return Vector3.zero;

			aimAtLocation = (aimAtLocation - this.transform.position).normalized;
			return GetAdjustedAim(aimAtLocation);
		}

		private Vector3 GetAdjustedAim(Vector3 vect)
		{
			bool above = (vect.y >= 0);
			float angle = Vector3.Angle(vect, transform.right);
			
			//get Adjusted angle
			//angle snapped to closes 45degrees
			angle = AimSnapAngle * (float)Mathf.Round(angle / (AimSnapAngle));
			
			if(!above)
				angle *= -1;
			
			Vector3 newVect = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
			
			//convert back to vector
			return newVect;
		}

        public override void InitiatePawnTypeVisuals(int team)
        {
            Material newMat = Resources.Load("HuskMat", typeof(Material)) as Material;

            Renderer renderer = GetPawnModel().GetComponent<Renderer>();
            renderer.material = newMat;
            //renderer.material.SetColor("_MainColor", GameInfo.GetTeamColor(team));
        }

        public override int GetAwardedKillAmount()
        {
            return KillAwardAmount;
        }

		public override PawnTypeEnum pawnTypeEnum
		{
			get{ return PawnTypeEnum.HUSK;}
		}
		
		public override Vector3 GetDirectionVector(int index)
		{
			return Vector3.zero;
		}
		
		public override Vector3 GetAimDirection(int index)
		{
			return GetHuskAutoAimDirection();
		}
		
		public override bool GetJump(int index)
		{
			return false;
		}
		
		public override bool GetFire(int index)
		{
            if(AutoFire && hostileTarget != null)
            {
                RaycastHit hit;
                if(Physics.Raycast(this.transform.position, hostileTarget.transform.position - this.transform.position, out hit) 
                   && hit.transform.tag != "Wall")
                {
                    return true;
                }
            }
            return false;
		}
	}
}
