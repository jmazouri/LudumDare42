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
        void ClearVelocityAndInput();
        void TakeDamage(float damage);
    }
}