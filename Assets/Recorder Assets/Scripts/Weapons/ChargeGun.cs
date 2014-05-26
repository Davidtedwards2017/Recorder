using UnityEngine;
using System.Collections;
using PawnFramework;

public class ChargeGun : aGunBase {

    private float chargeLevel = 0;
    public float chargeRate;

    public float MidCharge;
    public float MaxCharge;

    public Transform PartialChargedBulledPrefab;
    public Transform FullChargedBulletPrefab;

    void Update()
    {
        if(fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    protected override void TriggerPulled()
    {
        //start charge cooldown;
    }
    protected override void TriggerHeld()
    {
        if(fireCooldown > 0)
        {
            return;
        }

        chargeLevel += chargeRate * Time.deltaTime;

        if (chargeLevel > MaxCharge)
        {
            chargeLevel = MaxCharge;
        }
    }
    protected override void TriggerReleased()
    {
        if(fireCooldown > 0)
        {
            return;
        }

        FireShot();
        fireCooldown = fireDelay;
    }

    private void FireShot()
    {
        Debug.Log("charge shot fired ["+ chargeLevel+"]");

        Transform bullet;

        if(chargeLevel >= MaxCharge)
        {
            bullet = FullChargedBulletPrefab;
        }
        else if ( chargeLevel >= MidCharge)
        {
            bullet = PartialChargedBulledPrefab;
        }
        else
        {
            bullet = BulletPrefab;
        }
        SpawnBullet(bullet);
        chargeLevel = 0;
    }

}
