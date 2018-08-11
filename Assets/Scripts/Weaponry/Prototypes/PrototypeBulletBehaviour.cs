using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
using LD42.Weaponry;
using UnityEngine;

public class PrototypeBulletBehaviour : MonoBehaviour, IProjectile
{
    private Rigidbody2D _rb;

    [SerializeField] private float _speed;
    private bool _hasNotMoved;

    public float Damage { get; set; }

    void Start()
     {
         _rb = GetComponent<Rigidbody2D>();
         _hasNotMoved = true;
     }

    public Vector3? Target { get; set; }

    private void FixedUpdate()
    {
        if (Target.HasValue && _hasNotMoved)
        {
            _hasNotMoved = false;
            var direction = (Target.Value - transform.position).normalized;
            _rb.velocity = direction * _speed;
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerController = other.gameObject.GetComponent<IPlayerController>();
            playerController.TakeDamage(Damage);   
        }
        
        Destroy(gameObject);
    }
}