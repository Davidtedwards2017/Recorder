using UnityEngine;
using System.Collections;

public class ReflectiveStatus : AStatusBase {

    protected override void StartStatus()
    {
        //show visual effect
        
        pawn.OverriddenCollision = ReflectiveCollision;
    }
    
    protected override void StopStatus()
    {
        pawn.OverriddenCollision = null;
    }
    
    
    void ReflectiveCollision(Collision collision)
    {
        //OverriddenCollision
        if(collision.gameObject.tag.Equals("Projectile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
