using UnityEngine;
using System.Collections;
using PawnFramework;

public class Gun : aGunBase {

		
	void Update()
	{
		if(fireCooldown > 0)
			fireCooldown -= Time.deltaTime;
	}


	private void TryToFire()
	{
		if(fireCooldown > 0)
			return;

        SpawnBullet(BulletPrefab);
		fireCooldown = fireDelay;
	}

    protected override void TriggerPulled()
    {
        TryToFire();
    }
    protected override void TriggerHeld()
    {
        TryToFire();
    }
    protected override void TriggerReleased()
    {
    }

}
