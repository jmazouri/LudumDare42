using System.Dynamic;
using UnityEngine;
using LD42.PlayerControllers;
using Prime31;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : CharacterController2D, IPlayerController
{
    [SerializeField] private CharacterController2D _controller;

    [SerializeField] private float _health;

    [SerializeField] private Vector3 _velocity;
    private GameUIController _uiController;
    private SpriteRenderer _sprite;

    // Movement Config
    [SerializeField] [Range(-25, 25)] private float _gravity = -25f;
    [SerializeField] [Range(0, 20)] private float _jumpHeight = 3f;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _groundDamping = 20f; // how fast do we change direction? higher means faster
    [SerializeField] private float _inAirDamping = 5f;

    // Sound config
    [SerializeField] private AudioSource _jumpAudioSource;
    [SerializeField] private AudioSource _runningAudioSource;
    [SerializeField] private Animator _animator;

    public float Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            if (_health <= 0)
            {
                Destroy(gameObject);
                _uiController.UIState = UIState.GameOver;
            }
            else if (_health > 100)
            {
                _health = 100;
            }
        }
    }

    public Transform PlayerTransform { get; private set; }
    public Rigidbody2D PlayerRigidbody { get; private set; }
    public float MaxVelocityChange { get; set; } = 10.0f;

    public void TakeDamage(float damage)
    {
        Health -= damage;

        _uiController.AssignNewHealth(Health, 100);
    }

    public void Heal(float healAmount)
    {
        Health += healAmount;
        _uiController.AssignNewHealth(Health, 100);
    }

    void Start()
    {
        _uiController = GameObject.Find("GameHUD").GetComponent<GameUIController>();
       
        PlayerTransform = transform;
        PlayerRigidbody = GetComponent<Rigidbody2D>();

        _controller.onControllerCollidedEvent += OnControllerCollider;
        _controller.onTriggerEnterEvent += OnTriggerEnterEvent;
        _controller.onTriggerExitEvent += OnTriggerExitEvent;

        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnControllerCollider(RaycastHit2D hit)
    {
        if (hit.normal.y == 1f) return;
    }

    private void OnTriggerEnterEvent(Collider2D col)
    {
        //Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    private void OnTriggerExitEvent(Collider2D col)
    {
        //Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    void FixedUpdate()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
        }

        // Only jump when allowed to
        if (_controller.isGrounded && Input.GetButton("Jump"))
        {
            _velocity.y = Mathf.Sqrt(2f * _jumpHeight * -_gravity);
            _jumpAudioSource.Play();
        }

        var horizontalMovement = Input.GetAxis("Horizontal");
        if (_controller.isGrounded && horizontalMovement != 0 && !_runningAudioSource.isPlaying)
        {
            _runningAudioSource.Play();
        }
        else _runningAudioSource.Stop();

        var smoothMovementFactor = _controller.isGrounded ? _groundDamping : _inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, horizontalMovement * _speed, Time.deltaTime * smoothMovementFactor);

        _velocity.y += _gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);

        _sprite.flipX = _controller.velocity.x < 0;

        _velocity = _controller.velocity;

        _animator.SetFloat("VelocityMagnitude", Mathf.Abs(_velocity.x));
        _animator.SetBool("IsGrounded", _controller.isGrounded);
    }

    public void ClearVelocityAndInput()
    {
        _controller.move(new Vector3());
        _velocity = new Vector3();
        Input.ResetInputAxes();
    }
}