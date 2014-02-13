using UnityEngine;
using System.Collections;
using PawnFramework;
public class Station : MonoBehaviour {

	public Transform PawnPrefab;
	public Transform TimeBombPrefab;

	public Record StoredRecording;
	//public PawnController Pawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public PawnController SpawnPawn(PawnInfo pawnInfo)
	{
		Transform T = Instantiate(PawnPrefab, gameObject.transform.position, Quaternion.identity) as Transform;

		PawnController PawnCtrl;

		if(pawnInfo.player > 0)
		{
			PawnCtrl = T.gameObject.AddComponent<PlayerController>();
		}
		else //make this a recorded pawn
		{
			PawnCtrl = T.gameObject.AddComponent<RecordedController>();
			PawnCtrl.Recording = StoredRecording;
		}

		PawnCtrl.PawnDeathEvent += onPawnDeath;

		return PawnCtrl;

	}

	public TimeBomb SpawnTimeBomb(float lifeTime)
	{
		Transform T = Instantiate(TimeBombPrefab, gameObject.transform.position, Quaternion.identity) as Transform;

		TimeBomb bomb = T.GetComponent<TimeBomb>();
		bomb.LifeTime = lifeTime;

		return bomb;
	}

	#region events
	void onPawnDeath(PawnController pawn, GameObject killedBy)
	{
		if(pawn.isRecordedPawn())
		{

		}
		else
		{
			StoredRecording = pawn.Recording;
		}

		pawn.PawnDeathEvent -= onPawnDeath;
	}

	#endregion
}
