using System.Collections;
using System.Collections.Generic;
using LD42.AI.Prototypes;
using LD42.PlayerControllers;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour 
{
	public float FireRate = 1;
	public float Damage = 7;
    public float ProjectileDistance = 100;
	public LayerMask AffectedEntities;

	private float coolDown;
	private Transform projectile;
    private GameUIController _uiController;

    private int _ammo = 100;

    public int Ammo
    {
        get
        {
            return _ammo;
        }
        set
        {
            _ammo = value;
            _uiController.AssignNewAmmo(_ammo, MaxAmmo);
        }
    }
    public int MaxAmmo { get; set; } = 100;

    [SerializeField] private AudioSource _audioSource;

	void Awake() 
	{
		projectile = transform.Find("Laser");
        _uiController = GameObject.Find("GameHUD").GetComponent<GameUIController>();
    }
	
	void Update () 
	{
	    if (Input.GetButtonDown("Fire1"))
	    {
	        if (coolDown <= 0)
	        {
	            Shoot();
                coolDown = FireRate;
	        }
        }

        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }
    }

    LTDescr shootTween;

	void Shoot()
	{
		if (Ammo <= 0)
        {
            return;
        }

        Ammo--;

	    var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var mousePosition = new Vector2(point.x, point.y);

        var laserOrigin = new Vector2(projectile.position.x, projectile.position.y);

        var hit = Physics2D.Linecast(projectile.position, new Vector3(point.x, point.y, projectile.transform.position.z), AffectedEntities);

        var line = projectile.GetComponent<LineRenderer>();
        line.SetPosition(0, projectile.transform.position);
        line.SetPosition(1, new Vector3(point.x, point.y, projectile.transform.position.z));

        if (shootTween != null)
        {
            shootTween.setPassed(0.5f);
        }

        shootTween = LeanTween.value(projectile.gameObject,
        update =>
        {
            line.endColor = new Color(line.endColor.r, line.endColor.g, line.endColor.b, update);
            line.startColor = new Color(line.startColor.r, line.startColor.g, line.startColor.b, update);
        }, 1, 0, 0.5f)
        .setEase(LeanTweenType.easeInCirc);

        Debug.DrawLine(laserOrigin, (mousePosition - laserOrigin)*100, Color.red);

        _audioSource.Play();

	    if (hit.collider != null)
	    {
	        Debug.Log("Hit " + hit.collider.name);

            var enemy = hit.collider.gameObject.GetComponent<BasicEnemyBehaviour>();

	        if (enemy == null)
	            return;

            Debug.Log("Got enemy");

            enemy.TakeDamage(Damage);
	    }

        _uiController.AssignNewAmmo(Ammo, MaxAmmo);

    }
}
