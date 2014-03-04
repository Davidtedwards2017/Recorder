using UnityEngine;
using System.Collections;
using PawnFramework;
using StageFramework;

public class Bullet : MonoBehaviour {

	private Vector3 m_Direction;
	public PawnController owner;

	public float lifeTime;
	public float speed;
	public Vector3 direction
	{
		get { return m_Direction; }
		set { m_Direction = value.normalized; }
	}
	// Use this for initialization
	void Start () {
		direction = transform.forward;

		gameObject.layer = GameInfo.GetTeamProjLayerMask(owner.team);
		transform.GetComponent<Renderer> ().material.color = owner.GetComponentInChildren<Renderer>().material.color;

	}
	
	// Update is called once per frame
	void Update () {
	
		RaycastHit hit;
		if(Physics.Raycast(transform.position, m_Direction, out hit, speed * Time.deltaTime + 0.1f))
			ProjectileHit( hit.collider.gameObject, hit.normal, hit.normal);

		UpdateTrajectory();

		lifeTime -= Time.deltaTime;

		if(lifeTime < 0)
			Destroy(gameObject);
	}



	void OnCollisionEnter(Collision collision) 
	{
		ProjectileHit(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
	}

	void UpdateTrajectory()
	{

		Vector3 velocity = speed * direction * Time.deltaTime;
		velocity.z = 0;

		transform.LookAt(direction + transform.position);

		transform.position += velocity;
	
	}
	void OnDestroy() 
	{
		//TODO: spawn explosion effect
	}
	
	private void ProjectileHit(GameObject hitObject, Vector3 hitLoc, Vector3 hitNormal)
	{
		switch (hitObject.tag)
		{
		case "Wall":
			direction = Vector3.Reflect(direction, hitNormal);	
			return;
		case "Pawn":
			return;
		case "Projectile":
			//HitProjectile(hitObject, hitLoc, hitNormal);
			return;
		default:
			return;
		}
	}


}
