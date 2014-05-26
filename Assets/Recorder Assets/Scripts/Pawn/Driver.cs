using UnityEngine;
using System.Collections;
using StageFramework;

namespace PawnFramework
{
	[RequireComponent(typeof(PawnController))]
	public class Driver : PawnType {

		public bool bGamepadControls = true;
		public float AimSnapAngle = 45;
        public GameObject PlayerPointerEffectGO;

        public int KillAwardAmount = 500;

		private const string Keyboard_HorizontalAxis = "Horizontal";
		private const string Keyboard_VerticalAxis = "Vertical";
		private const string Keyboard_Jump = "Jump";
		private const string Keyboard_Fire = "Fire1";

        public PlayerInfo playerInfo;

        public Transform PointerEffectPrefab;
           

        protected override void Awake()
        {
            base.Awake();

            //PlayerPointerEffectGO = Instantiate(Resources.Load("PlayerPointerEffect"),transform.position, Quaternion.identity) as GameObject;
            //PlayerPointerEffectGO.transform.parent = gameObject.transform;
        }

        public override void InitiatePawnTypeVisuals(int team)
        {
            Material newMat = Resources.Load("DriverMat", typeof(Material)) as Material;
            Renderer renderer = GetPawnModel().GetComponent<Renderer>();
            renderer.material = newMat;
            //renderer.material.color = GameInfo.GetTeamColor(team);
        }

        void Start()
        {
            //PlayerPointerEffectGO.GetComponent<ParticleSystem>().startColor = GameInfo.GetTeamColor(playerInfo.team);
        }

        // Update is called once per frame
        void Update () 
        {
            if(!Pawn.bPlaying)
                return;


            if(GameInfo.Instance.CurrentGameMode.UseLifeTime)
            {
                CurrentLifeTime += Time.deltaTime;
                
                if(CurrentLifeTime >= EndLifeTime)
                {
                    Pawn.LifeTimeEnded();
                }
            }
        }

        void OnDestroy()
        {
            if(playerInfo == null) return;
            
            playerInfo.gameObject.MoveTo(GameInfo.Instance.SpawnQueue);
        }

        void onGameObjectAttached(object obj)
        {
            GameObject go = (GameObject) obj;
            if(go == null) return;
            
            if(go.name.Contains("Player_"))
            {
                playerInfo = go.GetComponent<PlayerInfo>();
            }
        }

		public override PawnTypeEnum pawnTypeEnum
		{
			get{ return PawnTypeEnum.DRIVER;}
		}

        public override int GetAwardedKillAmount()
        {
            return KillAwardAmount;
        }

		public override Vector3 GetDirectionVector(int index)
		{
            if(!Pawn.bPlaying || playerInfo == null)
				return Vector3.zero;

			Vector3 dirVec = PlayerGamepadInput.GetInputDirection (playerInfo.playerNum);
			dirVec.y = 0;
			dirVec.z = 0;
			GetRecordData(index).DirectionVector = dirVec;

			return dirVec;
		}

		public override Vector3 GetAimDirection(int index)
		{
            if(!Pawn.bPlaying || playerInfo == null)
				return Vector3.zero;

            Vector3 vec = PlayerGamepadInput.GetInputDirection (playerInfo.playerNum);
			
				if (vec == Vector3.zero)
					vec = Pawn.FacingDirection;
        
			vec = GetAdjustedAim(vec);

			GetRecordData(index).AimDirection = vec;

			return vec;
		}

		public override bool GetJump(int index)
		{
            if(!Pawn.bPlaying || playerInfo == null)
				return false;

            bool jump = PlayerGamepadInput.GetJump (playerInfo.playerNum);

			GetRecordData(index).Jump = jump;

			return jump;
		}
		public override bool GetFire(int index)
		{
            if(!Pawn.bPlaying || playerInfo == null)
				return false;

            bool fire = PlayerGamepadInput.GetFire(playerInfo.playerNum);
			GetRecordData(index).Fire = fire;
			return fire;
		}
		
		RecordData GetRecordData(int index)
		{
			if(Pawn.Recording.GetRecordData(index) == null)
				Pawn.Recording.AddRecordData();

			return Pawn.Recording.GetRecordData(index);
		}

		private Vector3 GetAdjustedAim(Vector3 vect)
		{
			bool above = (vect.y >= 0);
			float angle = Vector3.Angle(vect, transform.right);

			angle = AimSnapAngle * (float)Mathf.Round(angle / (AimSnapAngle));

			if(!above)
				angle *= -1;

			Vector3 newVect = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

			//convert back to vector
			return newVect;
		}

	}
}