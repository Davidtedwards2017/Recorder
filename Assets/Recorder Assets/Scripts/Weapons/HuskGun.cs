using UnityEngine;
using System.Collections;

public class HuskGun : aGunBase {
	
	private bool bCharging = false;
	public float currentChargingTime;

	public float ChargingTime = 1;

	// Update is called once per frame
	void Update () {
	
		if(fireCooldown > 0)
			fireCooldown -= Time.deltaTime;

		if(bCharging)
		{
			currentChargingTime -= Time.deltaTime;

			if( currentChargingTime <= 0)
				DoneCharging();
		}
	}

	private void StartCharging()
	{
		currentChargingTime = ChargingTime;
		bCharging = true;
	}

	private void DoneCharging()
	{
		bCharging = false;
		SpawnBullet(BulletPrefab);
		fireCooldown = fireDelay;
	}

    protected override void TriggerPulled()
    {
    }
    protected override void TriggerHeld()
    {
    }
    protected override void TriggerReleased()
    {
    }

    public override void Fire(bool pressed)
    {
        base.Fire(pressed);

        if(!pressed || fireCooldown > 0 || bCharging)
            return;
        
        StartCharging();
    }

	public override void SetGunDirection(Vector3 direction)
	{
		if(bCharging)
			return;

		direction = direction.normalized;
		transform.LookAt(transform.position + (direction));
	}





}
