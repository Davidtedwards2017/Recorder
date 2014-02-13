using UnityEngine;
using System.Collections;
using StageFramework;

namespace PawnFramework
{
	// Require a character controller to be attached to the same game object
	[RequireComponent(typeof(CharacterMotor))]
	[AddComponentMenu("Character/Platform Input Controller")]
	public abstract class PawnController : MonoBehaviour {

		public float LifeTime = 10;
		public Record Recording;
		public bool bPlaying;
		public bool autoRotate = true;
		public float maxRotationSpeed = 360.0f;
		public Gun gun;
		public int team;
		
		protected CharacterMotor motor;

		#region Event declaration
		public delegate void PawnDeathEventHandler(PawnController pawn, GameObject killedBy);
		public event PawnDeathEventHandler PawnDeathEvent;
		void onPawnDeath(PawnController pawn, GameObject killedBy) 
		{ 
			if(PawnDeathEvent != null) 
				PawnDeathEvent(pawn, killedBy); 
		}
		#endregion
		// Use this for initialization
		void Awake()
		{
			motor = GetComponent<CharacterMotor>();
			motor.SetControllable(false);

			gun = GetComponentInChildren<Gun>();

			//gameObject.layer = GameInfo.GetTeamPawnLayerMask(team);
			//ignore collisions from projectiles spawned from your team
			//Physics.IgnoreCollision(

		}

		// Update is called once per frame
		void Update()
		{
			// Get the input vector from kayboard or analog stick
			Vector3 directionVector = GetDirectionVector();
			
			if (directionVector != Vector3.zero)
			{
				// Get the length of the directon vector and then normalize it
				// Dividing by the length is cheaper than normalizing when we already have the length anyway
				var directionLength = directionVector.magnitude;
				directionVector = directionVector / directionLength;
				
				// Make sure the length is no bigger than 1
				directionLength = Mathf.Min(1, directionLength);
				
				// Make the input vector more sensitive towards the extremes and less sensitive in the middle
				// This makes it easier to control slow speeds when using analog sticks
				directionLength = directionLength * directionLength;
				
				// Multiply the normalized direction vector by the modified length
				directionVector = directionVector * directionLength;
			}
			
			// Rotate the input vector into camera space so up is camera's up and right is camera's right
			directionVector = Camera.main.transform.rotation * directionVector;
			
			// Rotate input vector to be perpendicular to character's up vector
			Quaternion camToCharacterSpace = Quaternion.FromToRotation(-Camera.main.transform.forward, transform.up);
			directionVector = (camToCharacterSpace * directionVector);
			
			// Apply the direction to the CharacterMotor
			motor.inputMoveDirection = directionVector;
			motor.inputJump = GetJump();
			
			// Set rotation to the move direction	
			if (autoRotate && directionVector.sqrMagnitude > 0.01)
			{
				Vector3 newForward = ConstantSlerp(transform.forward, directionVector, maxRotationSpeed * Time.deltaTime);
				newForward = ProjectOntoPlane(newForward, transform.up);
				transform.rotation = Quaternion.LookRotation(newForward, transform.up);
			}

			gun.SetGunDirection(GetAimDirection());

			if(GetFire())
				gun.FireGun();
		}

		void OnCollisionEnter(Collision collision) 
		{
			if(collision.gameObject.tag.Equals("Projectile"))
			{
				DespawnPawn();
				Destroy(collision.gameObject);
			}

		}
		
		Vector3 ProjectOntoPlane(Vector3 v, Vector3 normal)
		{
			return v - Vector3.Project(v, normal);
		}
		
		Vector3 ConstantSlerp(Vector3 from, Vector3 to, float angle)
		{
			float value = Mathf.Min(1, angle / Vector3.Angle(from, to));
			return Vector3.Slerp(from, to, value);
		}

		void OnDestroy() 
		{
			DespawnPawn();
		}

		protected void DespawnPawn()
		{
			onPawnDeath(this, null);

			//TODO: display despawn effects
			Destroy(gameObject);
		}

		public virtual void SetPlayable(bool value)
		{
			motor.SetControllable(value);
			bPlaying = value;
		}

		public void SetTeam(int team)
		{
			this.team = team;
			gameObject.layer = GameInfo.GetTeamPawnLayerMask(team);
			transform.GetComponentInChildren<Renderer> ().material.color = GameInfo.GetTeamColor(team);
		}

		public abstract bool isRecordedPawn();
		public abstract Vector3 GetDirectionVector();
		public abstract Vector3 GetAimDirection();
		public abstract bool GetJump();
		public abstract bool GetFire();
	}
}