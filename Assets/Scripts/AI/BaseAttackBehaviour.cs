using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
using UnityEngine;

namespace LD42.AI
{
    public abstract class BaseAttackBehaviour : MonoBehaviour
    {
        [SerializeField] protected float _damage;
        [SerializeField] protected float _cooldownTime;
        protected IPlayerController _target;
        protected float _coolDownTimePassed;
        public bool IsAttacking { get; protected set; }
        public abstract void StartAttack(IPlayerController player);
        public abstract void StopAttack();
    }
}