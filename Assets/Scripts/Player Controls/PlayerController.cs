using UnityEngine;
using System.Collections;
using LD42.PlayerControllers;

public class PlayerController : MonoBehaviour, IPlayerController
{
    
    public float Speed;
    private Rigidbody2D body;
    [SerializeField] private float _health;

    public float Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public Transform PlayerTransform { get; private set; }
    
    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        PlayerTransform = transform;
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis ("Horizontal");
        var vertical = Input.GetAxis ("Vertical");

        var movement = new Vector2 (horizontal, vertical);

        body.AddForce(movement * Speed);
    }
}