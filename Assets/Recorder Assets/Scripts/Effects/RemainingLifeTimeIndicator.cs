using UnityEngine;
using System.Collections;
using PawnFramework;

public class RemainingLifeTimeIndicator : MonoBehaviour {

	private tk2dSpriteAnimator animator;

	public tk2dSprite spriteScript;
	public int MaxIndex = 46;
	public float MaxLifeTime = 10f;
	public PawnController pawn;

	public float framesPerSecond = 30f;

	public int targetIndex;
	public int currentIndex;
	public float CurrentLifeTime;

	// Use this for initialization


	void Awake()
	{
		spriteScript = GetComponent<tk2dSprite>();
		animator = GetComponent<tk2dSpriteAnimator>();
	}
	void Start () {
		pawn = transform.parent.GetComponent<PawnController>();
		currentIndex = MaxIndex;
		StartCoroutine(updateClock());
	}
	
	// Update is called once per frame
	void Update () {

        if(!pawn.bPlaying)
        {
            return;
        }

        targetIndex = GetIndex();
	}

	private int GetIndex()
	{
        PawnType pType = pawn.pawnType;
        int index = 0;
        switch (pawn.GetPawnType())
        {
            case PawnTypeEnum.DRIVER:
                index = (MaxIndex - 1) - (int)(( ((Driver)pType).CurrentLifeTime * MaxIndex) / ((Driver)pType).EndLifeTime);
                break;
            case PawnTypeEnum.MIMIC:
                index =  (MaxIndex - 1) - (int)((pawn.TimeIndex * MaxIndex) / pawn.Recording.Length);
                
                break;
            default:
                break;
        }
        return index;
		//CurrentLifeTime = pawn.EndLifeTime;
		//return (int)(((pawn.EndLifeTime - pawn.CurrentLifeTime) / MaxLifeTime) * MaxIndex);
	}

	private IEnumerator updateClock()
	{
		while(true)
		{
			if(currentIndex != targetIndex)
			{
				//currentIndex += (currentIndex < targetIndex) ? 1 : -1;
				animator.SetFrame(targetIndex);
				currentIndex = targetIndex;
				//spriteScript.SetSprite(currentIndex);
			}

			yield return new WaitForSeconds( 1f / framesPerSecond);
		}
	}
}
