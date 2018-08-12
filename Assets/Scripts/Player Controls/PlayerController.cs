using UnityEngine;
using LD42.PlayerControllers;
using Prime31;

public class PlayerController : CharacterController2D, IPlayerController
{
    private CharacterController2D _controller;

    [SerializeField] private float _health;

    private Vector3 _velocity;

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
        _controller = GetComponent<CharacterController2D>();
        PlayerTransform = transform;
        PlayerRigidbody = GetComponent<Rigidbody2D>();

        _controller.onControllerCollidedEvent += OnControllerCollider;
        _controller.onTriggerEnterEvent += OnTriggerEnterEvent;
        _controller.onTriggerExitEvent += OnTriggerExitEvent;
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
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
        }

        // Only jump when allowed to
        if (_controller.isGrounded && Input.GetButton("Jump"))
        {
            _velocity.y = Mathf.Sqrt(2f * _jumpHeight * -_gravity);
        }

        var smoothMovementFactor = _controller.isGrounded ? _groundDamping : _inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, Input.GetAxis("Horizontal") * _speed, Time.deltaTime * smoothMovementFactor);

        _velocity.y += _gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);

        _velocity = _controller.velocity;
    }

    public void ClearVelocityAndInput()
    {
        _controller.move(new Vector3());
        _velocity = new Vector3();
        Input.ResetInputAxes();
    }
}