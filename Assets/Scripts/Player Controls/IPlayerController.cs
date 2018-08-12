using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

namespace LD42.PlayerControllers
{
    public interface IPlayerController
    {
        float Health { get; }
        Transform PlayerTransform { get; }
        CharacterController2D Controller { get; }
        Vector3 PlayerVelocity { get; set; }
        void TakeDamage(float damage);
    }
}