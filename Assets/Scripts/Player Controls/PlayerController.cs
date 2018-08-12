using UnityEngine;
using LD42.PlayerControllers;
using Prime31;

public class PlayerController : CharacterController2D, IPlayerController
{
    public CharacterController2D Controller { get; private set; }

    [SerializeField] private float _health;

    private Vector3 _velocity;
    public Vector3 PlayerVelocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    // Movement Config
    [SerializeField] [Range(-25, 25)] private float _gravity = -25f;
    [SerializeField] [Range(0, 20)] private float _jumpHeight = 3f;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _groundDamping = 20f; // how fast do we change direction? higher means faster
    [SerializeField] private float _inAirDamping = 5f;

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
    public float MaxVelocityChange { get; set; } = 10.0f;

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    void Start()
    {
        Controller = GetComponent<CharacterController2D>();
        PlayerTransform = transform;
        PlayerRigidbody = GetComponent<Rigidbody2D>();

        Controller.onControllerCollidedEvent += OnControllerCollider;
        Controller.onTriggerEnterEvent += OnTriggerEnterEvent;
        Controller.onTriggerExitEvent += OnTriggerExitEvent;
    }

    private void OnControllerCollider(RaycastHit2D hit)
    {
        if (hit.normal.y == 1f) return;
    }

    private void OnTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    private void OnTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    void Update()
    {
        if (Controller.isGrounded)
        {
            _velocity.y = 0;
        }

        // Only jump when allowed to
        if (Controller.isGrounded && Input.GetButton("Jump"))
        {
            _velocity.y = Mathf.Sqrt(2f * _jumpHeight * -_gravity);
        }

        var smoothMovementFactor = Controller.isGrounded ? _groundDamping : _inAirDamping;
        _velocity.x = Mathf.Lerp(PlayerVelocity.x, Input.GetAxis("Horizontal") * _speed, Time.deltaTime * smoothMovementFactor);

        _velocity.y += _gravity * Time.deltaTime;

        Controller.move(PlayerVelocity * Time.deltaTime);

        _velocity = Controller.velocity;
    }
}