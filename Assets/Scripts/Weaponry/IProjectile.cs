using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD42.Weaponry
{
    public interface IProjectile
    {
        Vector3? Target { get; set; }
        float Damage { get; set; }
    }
}