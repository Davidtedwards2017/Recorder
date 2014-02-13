using UnityEngine;
using System.Collections;
using PawnFramework;

public class Gun : MonoBehaviour {

	private float fireCooldown;
	private Transform Socket;

	public Transform BulletPrefab;
	public float fireDelay;

	// Use this for initialization
	void Start () 
	{
		Socket = transform.Find("Gun_Socket");
	}
	
	void Update()
	{
		if(fireCooldown > 0)
			fireCooldown -= Time.deltaTime;

	}

	public void SetGunDirection(Vector3 direction)
	{
		direction = direction.normalized;
		transform.LookAt(transform.position + (direction));
	}



	public void FireGun()
	{
		if(fireCooldown > 0)
			return;

		SpawnBullet();
		fireCooldown = fireDelay;
	}

	private void SpawnBullet()
	{
		Transform T = Instantiate(BulletPrefab, Socket.position, Socket.rotation) as Transform;
		T.gameObject.GetComponent<Bullet>().owner = transform.root.gameObject.GetComponent<PawnController>();
	}


}
