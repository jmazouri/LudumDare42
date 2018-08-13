using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour 
{
	public float FireRate = 0;
	public float Damage = 10;
    public float ProjectileDistance = 100;
	public LayerMask AffectedEntities;

	private float timeToFire = 0;
	private Transform projectile;

    [SerializeField] private AudioSource _audioSource;

	void Awake() 
	{
		projectile = transform.Find("Laser");
	}
	
	void Update () 
	{
	    if (Input.GetButtonDown("Fire1"))
	    {
	        if (FireRate <= 0)
	        {
	            Shoot();
	        }
	        else
	        {
	            if (Time.time > timeToFire)
	            {
	                timeToFire = Time.time + 1 / FireRate;
	                Shoot();
	            }
	        }
        }
	}

	void Shoot()
	{
		Debug.Log("Test");

	    var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var mousePosition = new Vector2(point.x, point.y);

        var laserOrigin = new Vector2(projectile.position.x, projectile.position.y);

	    var hit = Physics2D.Raycast(laserOrigin, mousePosition - laserOrigin, ProjectileDistance, AffectedEntities);

        Debug.DrawLine(laserOrigin, (mousePosition - laserOrigin)*100, Color.red);

        _audioSource.Play();

	    if (hit.collider != null)
	    {
            Debug.Log("Hit " + hit.collider.name);
	    }
	}
}
