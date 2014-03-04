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
	public float currentLifeTime;

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
		targetIndex = GetIndexFromRemainingLifeTime();
	}

	private int GetIndexFromRemainingLifeTime()
	{
		currentLifeTime = pawn.EndLifeTime;
		return (int)(((pawn.EndLifeTime - pawn.CurrentLifeTime) / MaxLifeTime) * MaxIndex);
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
