using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD42.Shrinking.Prototypes
{
    public class Shrink : MonoBehaviour
    {
        [SerializeField] private float _shrinkAmount = 1f;
        private float _resultScaleX;
        private float _resultScaleY;
        
        void Start ()
        {
            _resultScaleX = transform.localScale.x / _shrinkAmount;
            _resultScaleY = transform.localScale.y / _shrinkAmount;
            var resultScale = new Vector3(_resultScaleX, _resultScaleY);
            transform.localScale = resultScale;
        }
    }
}
