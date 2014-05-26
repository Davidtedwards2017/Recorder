using UnityEngine;
using System.Collections;
using StageFramework;
using PawnFramework;


public enum PawnTypeEnum { NONE, HUSK, DRIVER, MIMIC };
public delegate void CollisionDelegate(Collision collision);

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/Platform Input Controller")]
public class PawnController : MonoBehaviour {

    public int TimeIndex;
	public PawnType pawnType;
	public Station station;
	public int killCount = 0;
    public int NumberOfJumps = 2;

    public AudioClip sfxExplosion;
    public float sfxExplosionVolume;

	public float ExplosionEffectDuration = 3;
	public Transform ExplosionEffecPrefab;
	//public float EndLifeTime = 10;
	//public float CurrentLifeTime = 0;
	public Record Recording;
	public bool bPlaying;
	public bool autoRotate = true;
	public float maxRotationSpeed = 360.0f;
	private aGunBase m_gun;

    public TeamInfo teamInfo;
	//public int team;

    public Vector3 FacingDirection;

    public CollisionDelegate OverriddenCollision;
	
	protected CharacterMotor motor;

	public aGunBase gun
	{ 
		get 
		{   
			return GetComponentInChildren<aGunBase>();
		}
	}

	#region Event declaration
	public delegate void PawnDeathEventHandler(PawnController pawn, PawnController killedBy);
	public delegate void LifeTimeExausedEventHandler(PawnController pawn);
	public event PawnDeathEventHandler PawnDeathEvent;
	public event LifeTimeExausedEventHandler LifeTimeExausedEvent;

	void onPawnDeath(PawnController pawn, PawnController killedBy) 
	{ 
        if(PawnDeathEvent != null) 
			PawnDeathEvent(pawn, killedBy); 

		if(killedBy != null)
			killedBy.KilledPawn(this);
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
	}

	// Update is called once per frame
	void Update()
	{
		//if game is paused
		if(Time.deltaTime <= 0)
			return;

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

		if(directionVector != Vector3.zero)
			FacingDirection = directionVector.normalized;

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

		if(gun != null)
		{
			gun.SetGunDirection(GetAimDirection());

            gun.Fire(GetFire());
		}

		if(bPlaying && GetPawnType() != PawnTypeEnum.HUSK)
		{
			TimeIndex += 1;

			//CurrentLifeTime += Time.deltaTime;

            if(pawnType.pawnTypeEnum == PawnTypeEnum.MIMIC && TimeIndex >= Recording.Length)
			{
				LifeTimeEnded();
			}
		}

	}

	void OnCollisionEnter(Collision collision) 
	{
        if(OverriddenCollision != null)
        {
            OverriddenCollision(collision);
        }
        else
        {
            OnBaseCollision(collision);
        }
	}

    void OnBaseCollision(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Projectile"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            
            if(GetPawnType() == PawnTypeEnum.DRIVER)
            {
                GameInfo.Instance.CurrentGameMode.Score.ResetTeamScoreMultiplier(teamInfo.TeamNumber);
            }
            
            onPawnDeath(this, bullet.owner);
            PlayExplosionEffect();
            AudioHelper.CreatePlayAudioObject(sfxExplosion,sfxExplosionVolume);
            
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

	public void PlayExplosionEffect()
	{
		if(ExplosionEffecPrefab == null)
			return;

		Transform effectTransform = Instantiate(ExplosionEffecPrefab, transform.position, Quaternion.identity) as Transform;

        effectTransform.gameObject.GetComponent<TeamColorAdopter>().SetColor(teamInfo.TeamColor);

		Destroy(effectTransform.gameObject, ExplosionEffectDuration);
	}

	public void SetTeam(int team)
	{
        teamInfo = TeamInfo.AddTeamInfo(this.gameObject, team);
		
        pawnType.InitiatePawnTypeVisuals(team);
		gameObject.layer = GameInfo.GetTeamPawnLayerMask(team);
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

	public void LifeTimeEnded()
	{
		bPlaying = false;
		//ChangePawnType(PawnTypeEnum.HUSK);
		onLifeTimeExausted();
	}

    private int GetAdjustedKillAwardAmount()
    {
        return pawnType.GetAwardedKillAmount();
    }

    public void InitiateForPawnType<T>() where T : PawnType
    {
        //destory existing PawnType;
        Component.Destroy(gameObject.GetComponent<PawnType>());

        pawnType = gameObject.AddComponent<T>();

        switch (pawnType.pawnTypeEnum)
        {
            case PawnTypeEnum.DRIVER:

                if(GameInfo.Instance.CurrentGameMode.UseLifeTime)
                {
                    AddPawnTimeIndicatorEffect();
                }
                                    
                if(Recording == null)
                    Recording = new Record();
                
                break;

            case PawnTypeEnum.MIMIC:

                AddPawnTimeIndicatorEffect();

                TimeIndex = 0;
                //CurrentLifeTime = 0;
                break;

            case PawnTypeEnum.HUSK:

                SetTeam(0);
                SetPlayable(false);

                RemovePawnTimeIndicatorEffect();
                
                if(Recording == null)
                    Recording = new Record();
                
                TimeIndex = Recording.Length;
                
                break;
        }
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
        {
            ((Driver)pawnType).EndLifeTime += amount;
        }
			//EndLifeTime+= amount;
	}

	public void KilledPawn(PawnController pawnThatWasKilled)
	{
        //add to score for this pawns team if it is not a Husk
        if(GetPawnType() != PawnTypeEnum.HUSK)
        {
            GameInfo.Instance.CurrentGameMode.Score.AddToTeamScore(teamInfo.TeamNumber, pawnThatWasKilled.GetAdjustedKillAwardAmount());
        }

        if(GetPawnType() == PawnTypeEnum.DRIVER)
        {
            PlayerInfo player = gameObject.GetComponentInChildren<PlayerInfo>();
            player.KilledPawn(pawnThatWasKilled);
        }

	}

    private void AddPawnTimeIndicatorEffect()
    {
        Vector3 offset = new Vector3(0,0,1);
        GameObject go = Instantiate(Resources.Load("RemainingLifeTimeIndicator"),transform.position + offset, Quaternion.identity) as GameObject;
        go.transform.name = "RemainingLifeTimeIndicator";
        go.transform.parent = gameObject.transform;        
    }

    private void RemovePawnTimeIndicatorEffect()
    {
        Transform t = transform.FindChild("RemainingLifeTimeIndicator");

        if(t != null)
        {
            Destroy(t.gameObject);
        }
    }


    public void SetEffectMaterial(Material effectMaterial)
    {
        Renderer renderer = transform.FindChild("Pawn Model").GetComponent<Renderer>();

        Material[] newMaterials;

        if(effectMaterial != null)
        {
            newMaterials = new Material[2];
            newMaterials[0] = renderer.materials[0];
            newMaterials[1] = effectMaterial;
        }
        else
        {
            newMaterials = new Material[1];
            newMaterials[0] = renderer.materials[0];
        }

        renderer.materials = newMaterials;
    }


	
}