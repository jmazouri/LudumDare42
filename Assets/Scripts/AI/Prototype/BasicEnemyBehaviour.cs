using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
using UnityEngine;

namespace LD42.AI.Prototypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class BasicEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private BaseAttackBehaviour _attackBehaviour;
        private Rigidbody2D _rb;
        private IPlayerController _target;

        // Use this for initialization
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
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
        }

        private void HandleMove(GameObject playerObj)
        {
                if (playerObj.transform.position.x < transform.position.x && _rb.velocity.x > - _moveSpeed)
                {
                    _rb.AddForce(Vector2.left * _moveSpeed);
                }
                else if (playerObj.transform.position.x > transform.position.x && _rb.velocity.x < _moveSpeed)
                {
                    _rb.AddForce(Vector2.right * _moveSpeed);
                }
            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _target = other.GetComponent<IPlayerController>();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _target = null;
        }
    }
}
