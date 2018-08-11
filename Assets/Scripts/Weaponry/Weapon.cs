using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour 
{
	public float FireRate = 0;
	public float Damage = 10;
	public LayerMask ImmuneEntities;

	private float timeToFire = 0;
	private Transform projectile;

	void Awake() 
	{
		projectile = transform.FindChild("Laser");
	}
	
	void Update () 
	{
		if(FireRate == 0)
		{
			if(Input.GetButtonDown("Fire1"))
			{
				Shoot();
			}
		}
		else
		{
			if(Input.GetButton("Fire1") && Time.time > timeToFire)
			{
				timeToFire = Time.time + 1 / FireRate;
				Shoot();
			}
		}
	}

	void Shoot()
	{
		Debug.Log("Test");
	}
}
