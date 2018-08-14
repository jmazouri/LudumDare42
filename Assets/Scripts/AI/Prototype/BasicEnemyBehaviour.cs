using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
using Prime31;
using UnityEngine;

namespace LD42.AI.Prototypes
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicEnemyBehaviour : CharacterController2D
    {
        [SerializeField] private float _health;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private BaseAttackBehaviour _attackBehaviour;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private IPlayerController _target;
        private Ammo _ammoBoxPrefab;

        private CharacterController2D _controller;
        private Vector3 _velocity;

        // Movement config
        [SerializeField] [Range(-25, 25)] private float _gravity = -25f;
        [SerializeField] [Range(0, 20)] private float _jumpHeight = 3f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _groundDamping = 20f; // how fast do we change direction? higher means faster
        [SerializeField] private float _inAirDamping = 5f;

        private void SpawnAmmo()
        {
            Instantiate(_ammoBoxPrefab, transform.position, transform.rotation);
        }

        // Use this for initialization
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _controller = GetComponent<CharacterController2D>();
            _ammoBoxPrefab = Resources.Load<Ammo>("AmmoBox");
        }

        void FixedUpdate()
        {
            if (_target != null && Vector3.Distance(transform.position, _target.PlayerTransform.position) > _attackRadius)
            {
                HandleMove(_target.PlayerTransform.gameObject);
            }
            else if(!_attackBehaviour.IsAttacking)
            {
                _attackBehaviour.StartAttack(_target);
            }

            if (_attackBehaviour.IsAttacking && (_target == null || Vector3.Distance(transform.position, _target.PlayerTransform.position) > _attackRadius))
            {
                _attackBehaviour.StopAttack();
            }

            var result = Physics2D.OverlapCircleAll(transform.position, 10);

            foreach (var worldObject in result)
            {
                if (!worldObject.CompareTag("Player")) continue;
                
                _target = worldObject.GetComponent<IPlayerController>();
                return;
            }

            _target = null;
        }

        protected virtual void HandleMove(GameObject playerObj)
        {
            float leftOrRight = 0;

            if (playerObj.transform.position.x < transform.position.x)
            {
                leftOrRight = -1;
            }
            else if (playerObj.transform.position.x > transform.position.x)
            {
                leftOrRight = 1;
            }
            
            if (_controller.isGrounded)
                _velocity.y = 0;

            var smoothMovementFactor = _controller.isGrounded ? _groundDamping : _inAirDamping;
            _velocity.x = Mathf.Lerp(_velocity.x, leftOrRight * _speed, Time.deltaTime * smoothMovementFactor);

            _velocity.y += _gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);

            _spriteRenderer.flipX = _velocity.x < 0;

            _velocity = _controller.velocity;
        }

        public virtual void TakeDamage(float damage)
        {
            _health -= damage;

            LeanTween.value(gameObject, update =>
            {
                _spriteRenderer.color = update;
            }, Color.red, Color.white, 0.2f);

            if (!(_health <= 0)) return;
            
            var number = Random.Range(0, 101);
            if (number >= 51)
            {
                SpawnAmmo();
            }

            FindObjectOfType<GameUIController>().IncreaseScore(100);
            Destroy(gameObject);
        }
    }
}
