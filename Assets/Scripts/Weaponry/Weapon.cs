using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	        if (_ammo > MaxAmmo)
	        {
		        _ammo = MaxAmmo;
	        }
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

        var hits = Physics2D.LinecastAll(projectile.position, new Vector3(point.x, point.y, projectile.transform.position.z), AffectedEntities);

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

        if (hits.Length > 0)
        {
            var firstHit = hits[0];
            if (firstHit.collider != null)
            {
                Debug.Log("Hit " + firstHit.collider.name);

                var enemy = firstHit.collider.gameObject.GetComponent<BasicEnemyBehaviour>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                }
            }

            foreach (var hit in hits.Skip(1))
            {
                var phys = hit.collider?.GetComponent<Rigidbody2D>();

                if (phys != null)
                {
                    phys.AddForce((mousePosition - laserOrigin), ForceMode2D.Impulse);
                    hit.collider.GetComponent<BasicEnemyBehaviour>().TakeDamage(0);
                }
            }
        }

        _uiController.AssignNewAmmo(Ammo, MaxAmmo);

    }
}
