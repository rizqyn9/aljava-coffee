using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Tappable : MonoBehaviour
    {
        [Header("Properties")]

        [Header("Debug")]
        public BoxCollider2D boxCollider2D;
        public Transform lastTransform;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            lastTransform = transform;
        }

        private void OnMouseDown()
        {
            TapAndTap.Instance.reqTap(this);
            transform.LeanScale(new Vector3(1.5f, 1.5f, 1), .3f);
        }

        public void transformTo(Transform _transform)
        {
            transform.LeanMove(_transform.position, 1f);
        }
    }
}
