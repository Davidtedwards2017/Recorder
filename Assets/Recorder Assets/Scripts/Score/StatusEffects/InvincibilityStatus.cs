using UnityEngine;
using System.Collections;

public class InvincibilityStatus : AStatusBase 
{

    public Material StatusMaterial;

    protected override void StartStatus()
    {
        //show visual effect

        pawn.OverriddenCollision = InvincibilityCollision;
        pawn.SetEffectMaterial(StatusMaterial);
    }

    protected override void StopStatus()
    {
        pawn.OverriddenCollision = null;
        pawn.SetEffectMaterial(null);
    }


    void InvincibilityCollision(Collision collision)
    {
        //OverriddenCollision
        if(collision.gameObject.tag.Equals("Projectile"))
        {
            Destroy(collision.gameObject);
        }
    }
    
}
