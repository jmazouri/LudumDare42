using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD42.Shrinking.Prototypes
{
    public class Shrink : MonoBehaviour
    {
        public float ShrinkAmount = 1f;
        
        private float _resultScaleX;
        private float _resultScaleY;
        
        void Start ()
        {
            
        }

        public void ShrinkItem()
        {
            _resultScaleX = transform.localScale.x / ShrinkAmount;
            _resultScaleY = transform.localScale.y / ShrinkAmount;
            var resultScale = new Vector3(_resultScaleX, _resultScaleY);
            transform.localScale = resultScale;
        }
    }
}
