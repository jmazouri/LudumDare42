using System.Collections;
using System.Collections.Generic;
using LD42.AI;
using LD42.PlayerControllers;
using LD42.Weaponry;
using UnityEngine;

public class RangedAttackBehaviour : BaseAttackBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;

    // Update is called once per frame
    void Update()
    {
        if (_target != null && _coolDownTimePassed >= _cooldownTime)
        {
            _coolDownTimePassed = 0;
            var projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation)
                .GetComponent<IProjectile>();
            projectile.Target = _target.PlayerTransform.position;
            projectile.Damage = _damage;
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