using UnityEngine;
using System.Collections;
using StageFramework;

namespace PawnFramework
{

	public enum PawnTypeEnum { NONE, HUSK, DRIVER, MIMIC };
	// Require a character controller to be attached to the same game object
	[RequireComponent(typeof(CharacterMotor))]
	[AddComponentMenu("Character/Platform Input Controller")]
	public class PawnController : MonoBehaviour {

		public int TimeIndex;
		public PawnType pawnType;
		public PlayerInfo playerInfo;
		public Station station;
		public int killCount = 0;

		public float EndLifeTime = 10;
		public float CurrentLifeTime = 0;
		public Record Recording;
		public bool bPlaying;
		public bool autoRotate = true;
		public float maxRotationSpeed = 360.0f;
		public Gun gun;
		public int team;
		
		protected CharacterMotor motor;

		#region Event declaration
		public delegate void PawnDeathEventHandler(PawnController pawn, PawnController killedBy);
		public delegate void LifeTimeExausedEventHandler(PawnController pawn);
		public event PawnDeathEventHandler PawnDeathEvent;
		public event LifeTimeExausedEventHandler LifeTimeExausedEvent;

		void onPawnDeath(PawnController pawn, PawnController killedBy) 
		{ 
			if(PawnDeathEvent != null) 
				PawnDeathEvent(pawn, killedBy); 
		}
		void onLifeTimeExausted()
		{
			if(LifeTimeExausedEvent != null)
				LifeTimeExausedEvent(this);
		}
		#endregion
		// Use this for initialization
		void Awake()
		{
			motor = GetComponent<CharacterMotor>();
			motor.SetControllable(false);

			gun = GetComponentInChildren<Gun>();
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
			//directionVector.z = 0; //clamp for 2d movement
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

			if(bPlaying && GetPawnType() != PawnTypeEnum.HUSK)
			{
				TimeIndex += 1;

				CurrentLifeTime += Time.deltaTime;

				if(CurrentLifeTime >= EndLifeTime || (pawnType.pawnTypeEnum == PawnTypeEnum.MIMIC && TimeIndex >= Recording.Length))
				{
					LifeTimeEnded();
				}
			}

		}

		void OnCollisionEnter(Collision collision) 
		{
			if(collision.gameObject.tag.Equals("Projectile"))
			{
				Bullet bullet = collision.gameObject.GetComponent<Bullet>();

				onPawnDeath(this, bullet.owner);
				Destroy(collision.gameObject);
				Destroy(gameObject);
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

		public bool isRecordedPawn()
		{
			return pawnType.isRecordedPawn();
		}
		private Vector3 GetDirectionVector()
		{
			return pawnType.GetDirectionVector(TimeIndex);
		}
		private Vector3 GetAimDirection()
		{
			return pawnType.GetAimDirection(TimeIndex);
		}
		private bool GetJump()
		{
			return pawnType.GetJump(TimeIndex);
		}
		private bool GetFire()
		{
			return pawnType.GetFire(TimeIndex);
		}

		private void LifeTimeEnded()
		{
			bPlaying = false;
			//ChangePawnType(PawnTypeEnum.HUSK);
			playerInfo = null;
			onLifeTimeExausted();
		}

		public void ChangePawnType(PawnTypeEnum type)
		{
			Component.Destroy(gameObject.GetComponent<PawnType>());

			switch (type)
			{
			case PawnTypeEnum.DRIVER:
				pawnType = gameObject.AddComponent<Driver>();

				if(Recording == null)
					Recording = new Record();

				break;
			case PawnTypeEnum.MIMIC:
				pawnType = gameObject.AddComponent<Mimic>();
				TimeIndex = 0;
				CurrentLifeTime = 0;
				break;
			case PawnTypeEnum.HUSK:
				pawnType = gameObject.AddComponent<Husk>();
				SetTeam(0);
				SetPlayable(false);

				if(Recording == null)
					Recording = new Record();

				TimeIndex = Recording.Length;

				break;
			}
		}

		public void PossessPawn(PlayerInfo playerInfo)
		{
			//TODO

			this.playerInfo = playerInfo;
			SetTeam(playerInfo.team);
		}

		public PawnTypeEnum GetPawnType()
		{
			if( pawnType == null)
				return PawnTypeEnum.NONE;

			return pawnType.pawnTypeEnum;
		}

		public void AddTime(float amount)
		{
			if(GetPawnType() == PawnTypeEnum.DRIVER)
				EndLifeTime+= amount;

			//if(EndLifeTime 
		}
	}
}