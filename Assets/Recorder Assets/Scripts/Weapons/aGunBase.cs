using UnityEngine;
using System.Collections;
using PawnFramework;

public abstract class aGunBase : MonoBehaviour {
	protected float fireCooldown;
	protected Transform Socket;

	protected bool alreadyPressed = false;
	
    public AudioClip sfxFire;
    public float sfxFireVolume;
	public Transform BulletPrefab;
	public float fireDelay;

    private Vector3 aimDirection;
	
	// Use this for initialization
	protected void Awake () 
	{
		Socket = transform.Find("Gun_Socket");

       // arm = transform.root.FindChild("rightArm").transform;

	}

   
	public virtual void SetGunDirection(Vector3 direction)
	{
		direction = direction.normalized;
		transform.LookAt(transform.position + (direction));



        //aimDirection = direction;
	}

	public virtual void Fire(bool pressed)
	{
        if (pressed && !alreadyPressed) //just pulled trigger
        {
            TriggerPulled();
        } 
        else if (pressed && alreadyPressed) //still holding down trigger
        {
            TriggerHeld();
        } 
        else if (!pressed && alreadyPressed) //just released trigger
        {
            TriggerReleased();
        }

        alreadyPressed = pressed;
	}

    protected abstract void TriggerPulled();
    protected abstract void TriggerHeld();
    protected abstract void TriggerReleased();

	protected virtual void SpawnBullet(Transform bulletPrefab)
	{
        Transform T = Instantiate(bulletPrefab, Socket.position, Socket.rotation) as Transform;
		T.gameObject.GetComponent<Bullet>().owner = transform.root.gameObject.GetComponent<PawnController>();

        AudioHelper.CreatePlayAudioObject(sfxFire,sfxFireVolume);
    }
	
}
