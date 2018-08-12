using UnityEngine;
using System.Collections;
using LD42.PlayerControllers;

public class PlayerController : MonoBehaviour, IPlayerController
{
    public float Speed;
    private Rigidbody2D body;
    [SerializeField] private float _health;
    private float _horizontal;
    private float _vertical;

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
    public Rigidbody2D PlayerRigidbody { get; private set; }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        PlayerTransform = transform;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        var movement = new Vector2(_horizontal, _vertical);

        body.AddForce(movement * Speed);
    }
}