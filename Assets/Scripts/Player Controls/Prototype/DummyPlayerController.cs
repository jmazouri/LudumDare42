using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD42.PlayerControllers.Prototypes
{
    public class DummyPlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private float _health;

        public float Health
        {
            get { return _health; }
            private set
            {
                _health = value;
                if (_health > 0) return;
                Destroy(gameObject);
            }
        }

        public Transform PlayerTransform => transform;

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }
    }
}