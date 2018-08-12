using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
using UnityEngine;

namespace LD42.AI.Prototypes
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private BaseAttackBehaviour _attackBehaviour;
        private Rigidbody2D _rb;
        private IPlayerController _target;
        private Ammo _ammoBoxPrefab;

        private void SpawnAmmo()
        {
            Instantiate(_ammoBoxPrefab, transform.position, transform.rotation);
        }

        // Use this for initialization
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
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
                if (playerObj.transform.position.x < transform.position.x && _rb.velocity.x > - _moveSpeed)
                {
                    _rb.AddForce(Vector2.left * _moveSpeed);
                }
                else if (playerObj.transform.position.x > transform.position.x && _rb.velocity.x < _moveSpeed)
                {
                    _rb.AddForce(Vector2.right * _moveSpeed);
                }
            
        }

        public virtual void TakeDamage(float damage)
        {
            _health -= damage;
            if (!(_health <= 0)) return;
            
            Destroy(gameObject);
            SpawnAmmo();
        }
    }
}
