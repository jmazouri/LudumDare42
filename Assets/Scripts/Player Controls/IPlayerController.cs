using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD42.PlayerControllers
{
    public interface IPlayerController
    {
        float Health { get; }
        Transform PlayerTransform { get; }
        void TakeDamage(float damage);
    }
}