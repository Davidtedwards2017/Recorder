using UnityEngine;
using System.Collections;
using PawnFramework;
using StageFramework;

public class Bullet : MonoBehaviour {

	private Vector3 m_Direction;
	public PawnController owner;
    public AudioClip BounceSoundsClip;
    public Transform BulletParticleEmitterPrefab;
    public Transform BulletBouncePE;
    public float BulletBouncePEDuration;
    public float BounceEffectOffset;
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

		gameObject.layer = GameInfo.GetTeamProjLayerMask(owner.teamInfo.TeamNumber);
        InitiateBulletParticleEmitter();
	}

    public void InitiateBulletParticleEmitter()
    {
        Transform PE = Instantiate(BulletParticleEmitterPrefab, transform.position, transform.rotation) as Transform;
        PE.parent = gameObject.transform;
        PE.name = "BulletPE";
    }
	
	// Update is called once per frame
	void Update () {
	

		RaycastHit hit;
		if(Physics.Raycast(transform.position, m_Direction, out hit, speed * Time.deltaTime))
        {
			ProjectileHit( hit.collider.gameObject, hit.point, hit.normal);
        }

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
        Transform PE = transform.FindChild("BulletPE");
        PE.particleSystem.Stop();
        PE.gameObject.transform.parent = null;
        Destroy(PE.gameObject, 0.2f); 
	}
	
	private void ProjectileHit(GameObject hitObject, Vector3 hitLoc, Vector3 hitNormal)
	{
        switch (hitObject.tag)
		{
		case "Wall":
			direction = Vector3.Reflect(direction, hitNormal);	
            SpawnBounceEffect(hitLoc, hitNormal);
            //GameObject.FindGameObjectWithTag("GameInfo").GetComponent<AudioSource>().PlayOneShot(BounceSoundsClip);
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

    private void SpawnBounceEffect(Vector3 location, Vector3 hitNormal)
    {
        if(BulletBouncePE == null && hitNormal != Vector3.zero)
            return;
        
        Transform effectTransform = Instantiate(BulletBouncePE, location + (hitNormal * BounceEffectOffset), Quaternion.LookRotation(hitNormal)) as Transform;
        //Quaternion.Euler(hitNormal)
        effectTransform.gameObject.GetComponentInChildren<TeamColorAdopter>().SetColor(owner.teamInfo.TeamColor);

        Destroy(effectTransform.gameObject, BulletBouncePEDuration);
    }

}
