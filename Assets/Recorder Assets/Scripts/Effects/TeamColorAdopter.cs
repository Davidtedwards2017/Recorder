using UnityEngine;
using System.Collections;

public class TeamColorAdopter : MonoBehaviour {

    // Use this for initialization
	void Start () {

        Transform root = transform.root;

        if(root.tag == "Projectile")
        {
            Bullet bullet = root.gameObject.GetComponent<Bullet>();
            if(bullet != null && bullet.owner != null)
            {
               root = bullet.owner.transform.root;
            }
        }
        else
        {
            root = transform.root;
        }

        TeamInfo teamInfo = root.transform.GetComponent<TeamInfo>();

        if(teamInfo != null)
        {
            SetColor(teamInfo.TeamColor);
        }
	}

    public virtual void SetColor(Color color)
    {
        renderer.material.color = color;

        foreach(ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.startColor = color;
        }

    }
	
}
