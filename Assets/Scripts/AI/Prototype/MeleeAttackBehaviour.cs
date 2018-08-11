using System.Collections;
using System.Collections.Generic;
using LD42.AI;
using LD42.PlayerControllers;
using UnityEngine;

namespace LD42.AI.Prototypes
{
    public class MeleeAttackBehaviour : BaseAttackBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (_target != null && _coolDownTimePassed >= _cooldownTime)
            {
                _target.TakeDamage(_damage);
                _coolDownTimePassed = 0;
            }
            else if (_coolDownTimePassed < _cooldownTime)
            {
                _coolDownTimePassed += Time.deltaTime;
            }
        }

        public override void StartAttack(IPlayerController player)
        {
            IsAttacking = true;
            _target = player;
        }

        public override void StopAttack()
        {
            _target = null;
            IsAttacking = false;
        }
    }
}